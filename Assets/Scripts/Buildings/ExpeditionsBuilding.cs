#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
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

        private static ArmyManager ArmyManager => Game.Instance.ArmyManager;

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
        public IReadOnlyReactiveProperty<int> WorkerCount => ArmyManager.ExplorersCount;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => ArmyManager.GuardsCount;
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
        private void Construct(FoodController foodController, MoneyController moneyController, Expeditions expeditions)
        {
            _foodController = foodController;
            _moneyController = moneyController;
            _expeditions = expeditions;
            _foodController.ArmyFood.Subscribe(_ => UpdateCanBeSentStatus());
            _moneyController.ArmyMoney.Subscribe(_ => UpdateCanBeSentStatus());
            _expeditions.IsExpeditionActive.Subscribe(_ => UpdateCanBeSentStatus());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ArmyManager.ExplorersCount
                .TakeUntilDisable(this)
                .Subscribe(OnExplorersCountChanged);
            Game.Instance.RoundSystem.Round
                .TakeUntilDisable(this)
                .Subscribe(OnRoundChanged);
        }

        private void OnRoundChanged(int round)
        {
            _expeditionsActive.Value = round >= _activeFromWeek;
        }

        public void AddWorker()
        {
            ArmyManager.AddArmyToExplorers();
        }

        public void RemoveWorker()
        {
            ArmyManager.RemoveArmyFromExplorers();
        }

        public bool CanAddWorker()
        {
            return ArmyManager.GuardsCount.Value > 0;
        }

        public bool CanRemoveWorker()
        {
            return ArmyManager.ExplorersCount.Value > 0;
        }

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

            int workersCount = ArmyManager.ExplorersCount.Value;
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