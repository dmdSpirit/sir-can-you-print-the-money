#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.Rounds;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class ExpeditionsBuilding : Building, IExpeditionSender, IProducer
    {
        private readonly ReactiveProperty<bool> _canBeSentToExpedition = new();
        private readonly ReactiveProperty<bool> _expeditionsActive = new();

        private FoodController _foodController = null!;
        private MoneyController _moneyController = null!;
        private Expeditions _expeditions = null!;
        private ArmyManager _armyManager = null!;
        private RoundSystem _roundSystem = null!;

        [SerializeField]
        private string _workerTitle = "Explorers";

        [SerializeField]
        private TMP_Text _numberOfExplorersText = null!;

        [SerializeField]
        private int _expeditionFoodPerPersonCost = 3;

        [SerializeField]
        private int _expeditionMoneyPerPersonCost = 0;

        [SerializeField]
        private int _activeFromWeek = 3;

        public override BuildingType BuildingType => BuildingType.Expeditions;
        public IReadOnlyReactiveProperty<int> WorkerCount => _armyManager.ExplorersCount;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => _armyManager.GuardsCount;
        public int MaxWorkerCount => 0;
        public bool HasMaxWorkerCount => false;
        public string WorkersTitle => _workerTitle;
        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => _expeditions.IsExpeditionActive;
        public IReadOnlyReactiveProperty<bool> CanBeSentToExpedition => _canBeSentToExpedition;
        public IReadOnlyTimer? ExpeditionTimer => _expeditions.Timer;
        public float WinProbability => _expeditions.GetExpeditionWinProbability();
        public int Defenders => _expeditions.GetCurrentExpeditionData().Defenders;
        public int Reward => _expeditions.GetCurrentExpeditionData().Reward;
        public int ExpeditionFoodPerPersonCost => _expeditionFoodPerPersonCost;
        public int ExpeditionMoneyPerPersonCost => _expeditionMoneyPerPersonCost;
        public bool ShowProducedValue => false;
        public IReadOnlyReactiveProperty<int>? ProducedValue => null;
        public IReadOnlyReactiveProperty<bool> IsProducing => _expeditions.IsExpeditionActive;
        public IReadOnlyTimer? ProductionTimer => _expeditions.Timer;
        public IReadOnlyReactiveProperty<bool> IsActive => _expeditionsActive;

        [Inject]
        private void Construct(FoodController foodController, MoneyController moneyController, Expeditions expeditions,
            ArmyManager armyManager, RoundSystem roundSystem)
        {
            _foodController = foodController;
            _moneyController = moneyController;
            _expeditions = expeditions;
            _armyManager = armyManager;
            _roundSystem = roundSystem;
            _foodController.ArmyFood.Subscribe(_ => UpdateCanBeSentStatus());
            _moneyController.ArmyMoney.Subscribe(_ => UpdateCanBeSentStatus());
            _expeditions.IsExpeditionActive.Subscribe(_ => UpdateCanBeSentStatus());
            _armyManager.ExplorersCount.Subscribe(OnExplorersCountChanged);
            _roundSystem.Round.Subscribe(OnRoundChanged);
        }

        private void OnRoundChanged(int round)
        {
            _expeditionsActive.Value = round >= _activeFromWeek;
        }

        public void AddWorker() => _armyManager.AddArmyToExplorers();

        public void RemoveWorker() => _armyManager.RemoveArmyFromExplorers();

        public bool CanAddWorker() => _armyManager.GuardsCount.Value > 0;

        public bool CanRemoveWorker() => _armyManager.ExplorersCount.Value > 0;

        private void UpdateCanBeSentStatus()
        {
            if (IsExpeditionActive.Value)
            {
                if (_canBeSentToExpedition.Value)
                {
                    _canBeSentToExpedition.Value = false;
                }

                return;
            }

            int workersCount = _armyManager.ExplorersCount.Value;
            _canBeSentToExpedition.Value =
                workersCount > 0
                && IsEnoughMoneyForExpedition()
                && IsEnoughFoodForExpedition();

            bool IsEnoughMoneyForExpedition() =>
                _moneyController.ArmyMoney.Value >= workersCount * _expeditionMoneyPerPersonCost;

            bool IsEnoughFoodForExpedition() =>
                _foodController.ArmyFood.Value >= workersCount * _expeditionFoodPerPersonCost;
        }

        private void OnExplorersCountChanged(int explorersCount)
        {
            _numberOfExplorersText.text = explorersCount.ToString();
            UpdateCanBeSentStatus();
        }
    }
}