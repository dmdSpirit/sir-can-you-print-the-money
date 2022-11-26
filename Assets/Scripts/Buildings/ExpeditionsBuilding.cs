#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ExpeditionsBuilding : Building, IExpeditionSender
    {
        private readonly ReactiveProperty<bool> _canBeSentToExpedition = new();

        private static ArmyManager ArmyManager => Game.Instance.ArmyManager;
        private static MoneyController MoneyController => Game.Instance.MoneyController;
        private static FoodController FoodController => Game.Instance.FoodController;

        [SerializeField]
        private string _workerTitle = "Explorers";

        [SerializeField]
        private TMP_Text _numberOfExplorersText = null!;

        [SerializeField]
        private int _expeditionFoodPerPersonCost = 3;

        [SerializeField]
        private int _expeditionMoneyPerPersonCost = 0;

        public override BuildingType BuildingType => BuildingType.Expeditions;
        public IReadOnlyReactiveProperty<int> WorkerCount => ArmyManager.ExplorersCount;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => ArmyManager.GuardsCount;
        public int MaxWorkerCount => 0;
        public bool HasMaxWorkerCount => false;
        public string WorkersTitle => _workerTitle;
        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => Game.Instance.Expeditions.IsExpeditionActive;
        public IReadOnlyReactiveProperty<bool> CanBeSentToExpedition => _canBeSentToExpedition;
        public IReadOnlyTimer? ExpeditionTimer => Game.Instance.Expeditions.Timer;
        public int ExpeditionFoodPerPersonCost => _expeditionFoodPerPersonCost;
        public int ExpeditionMoneyPerPersonCost => _expeditionMoneyPerPersonCost;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.MoneyController.ArmyMoney
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateCanBeSentStatus());
            Game.Instance.FoodController.ArmyFood
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateCanBeSentStatus());
            Game.Instance.Expeditions.IsExpeditionActive
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateCanBeSentStatus());
            ArmyManager.ExplorersCount
                .TakeUntilDisable(this)
                .Subscribe(OnExplorersCountChanged);
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
                MoneyController.ArmyMoney.Value >= workersCount * _expeditionMoneyPerPersonCost;

            bool IsEnoughFoodForExpedition() =>
                FoodController.ArmyFood.Value >= workersCount * _expeditionFoodPerPersonCost;
        }

        private void OnExplorersCountChanged(int explorersCount)
        {
            _numberOfExplorersText.text = explorersCount.ToString();
            UpdateCanBeSentStatus();
        }
    }
}