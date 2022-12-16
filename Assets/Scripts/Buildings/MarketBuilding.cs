#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class MarketBuilding : Building, IWorkerManipulator, IProducer
    {
        private readonly ReactiveProperty<bool> _isProducing = new();
        private readonly ReactiveProperty<int> _producedValue = new();

        private FolkManager _folkManager = null!;
        private FoodController _foodController = null!;
        private MoneyController _moneyController = null!;
        
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
        public IReadOnlyReactiveProperty<int> WorkerCount => _folkManager.MarketFolk;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => _folkManager.FarmFolk;
        public int MaxWorkerCount => _folkManager.MaxMarkerWorkers;
        public bool HasMaxWorkerCount => true;
        public string WorkersTitle => _workersTitle;
        public bool ShowProducedValue => true;
        public IReadOnlyReactiveProperty<int> ProducedValue => _producedValue;
        public IReadOnlyReactiveProperty<bool> IsProducing => _isProducing;
        public IReadOnlyTimer? ProductionTimer => _productionTimer;

        [Inject]
        private void Construct(FolkManager folkManager, FoodController foodController, MoneyController moneyController)
        {
            _folkManager = folkManager;
            _foodController = foodController;
            _moneyController = moneyController;
            _folkManager.MarketFolk.Subscribe(_ => UpdateProduction());
            _foodController.FolkFood.Subscribe(_ => UpdateProduction());
            _moneyController.ArmyMoney.Subscribe(_ => UpdateProduction());
        }

        public void AddWorker()
        {
            _folkManager.AddFolkToMarket();
        }

        public void RemoveWorker()
        {
            _folkManager.RemoveFolkFromMarket();
        }

        public bool CanAddWorker() => _folkManager.CanAddMarketWorker();

        public bool CanRemoveWorker() => _folkManager.CanRemoveMarketWorker();

        private void UpdateProduction()
        {
            _workerText.text = _folkManager.MarketFolk.Value.ToString();
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
            return _foodController.FolkFood.Value >= _foodChangedPerTrade
                   && _folkManager.MarketFolk.Value >= 1
                   && _moneyController.ArmyMoney.Value >= _foodChangedPerTrade * _foodCostPerUnit;
        }

        private void StartProduction()
        {
            Assert.IsTrue(_folkManager.MarketFolk.Value > 0);
            Assert.IsTrue(_productionTimer == null);
            _productionTimer = Game.Instance.TimeSystem.CreateTimer(_tradeDuration, OnTradeFinished);
            _productionTimer.Start();
            _producedValue.Value = _foodChangedPerTrade * _folkManager.MarketFolk.Value;
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
            Assert.IsTrue(_folkManager.MarketFolk.Value > 0);
            _moneyController.TransferMoneyFromArmyToFolk(_foodCostPerUnit * _foodChangedPerTrade);
            _foodController.TransferFoodFromFolkToArmy(_foodChangedPerTrade);
            _productionTimer = null;
            _isProducing.Value = false;
            StartProduction();
        }

    }
}