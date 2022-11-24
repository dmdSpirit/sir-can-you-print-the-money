#nullable enable
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Buildings
{
    public sealed class MarketBuilding : Building, IWorkerManipulator, IProducer
    {
        private readonly ReactiveProperty<bool> _isProducing = new();
        private readonly ReactiveProperty<int> _producedValue = new();
        
        private Timer? _productionTimer;

        [SerializeField]
        private TMP_Text _workerText = null!;

        [SerializeField]
        private float _tradeDuration = 5f;

        [SerializeField]
        private int _foodCostPerUnit = 2;

        [SerializeField]
        private int _foodChangedPerTrade = 1;

        [SerializeField]
        private string _workersTitle = "Traders";

        public override BuildingType BuildingType => BuildingType.Market;
        public IReadOnlyReactiveProperty<int> WorkerCount => Game.Instance.FolkManager.MarketFolk;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => Game.Instance.FolkManager.IdleFolk;
        public int MaxWorkerCount => Game.Instance.FolkManager.MaxMarkerWorkers;
        public bool HasMaxWorkerCount => true;
        public string WorkersTitle => _workersTitle;
        
        public IReadOnlyReactiveProperty<int> ProducedValue => _producedValue;
        public IReadOnlyReactiveProperty<bool> IsProducing => _isProducing;
        public Timer? ProductionTimer => _productionTimer;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FolkManager.MarketFolk
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateProduction());
            Game.Instance.FoodController.FolkFood
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateProduction());
            Game.Instance.MoneyController.ArmyMoney
                .TakeUntilDisable(this)
                .Subscribe(_ => UpdateProduction());
        }
        
        public void AddWorker()
        {
            Game.Instance.FolkManager.AddFolkToMarket();
        }

        public void RemoveWorker()
        {
            Game.Instance.FolkManager.RemoveFolkFromMarket();
        }
        
        public bool CanAddWorker()
        {
            return Game.Instance.FolkManager.IdleFolk.Value > 0
                   && Game.Instance.FolkManager.MarketFolk.Value < Game.Instance.FolkManager.MaxMarkerWorkers;
        }

        public bool CanRemoveWorker()
        {
            return Game.Instance.FolkManager.MarketFolk.Value > 0;
        }

        private void UpdateProduction()
        {
            _workerText.text = Game.Instance.FolkManager.MarketFolk.Value.ToString();
            if (_isProducing.Value && !IsAbleToTrade())
            {
                StopProduction();
                return;
            }

            if (!_isProducing.Value && IsAbleToTrade())
            {
                StartProduction();
            }
        }

        private bool IsAbleToTrade()
        {
            return Game.Instance.FoodController.FolkFood.Value >= _foodChangedPerTrade
                   && Game.Instance.FolkManager.MarketFolk.Value >= 1
                   && Game.Instance.MoneyController.ArmyMoney.Value >= _foodChangedPerTrade * _foodCostPerUnit;
        }

        private void StartProduction()
        {
            Assert.IsTrue(Game.Instance.FolkManager.MarketFolk.Value > 0);
            Assert.IsTrue(_productionTimer == null);
            _productionTimer = Game.Instance.TimeSystem.CreateTimer(_tradeDuration, OnTradeFinished);
            _productionTimer.Start();
            _producedValue.Value = _foodChangedPerTrade * Game.Instance.FolkManager.MarketFolk.Value;
            _isProducing.Value = true;
        }

        private void StopProduction()
        {
            if (!_isProducing.Value)
            {
                return;
            }

            _productionTimer?.Cancel();
            _productionTimer = null;
            _producedValue.Value = 0;
            _isProducing.Value = false;
        }

        private void OnTradeFinished(Timer _)
        {
            Assert.IsTrue(_productionTimer != null);
            Assert.IsTrue(_isProducing.Value);
            Assert.IsTrue(Game.Instance.FolkManager.MarketFolk.Value > 0);
            Game.Instance.MoneyController.TransferMoneyFromArmyToFolk(_foodCostPerUnit * _foodChangedPerTrade);
            Game.Instance.FoodController.TransferFoodFromFolkToArmy(_foodChangedPerTrade);
            _productionTimer = null;
            _isProducing.Value = false;
            StartProduction();
        }

    }
}