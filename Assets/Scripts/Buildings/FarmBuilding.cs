#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Buildings
{
    public sealed class FarmBuilding : Building, IWorkerManipulator, IProducer
    {
        private readonly ReactiveProperty<bool> _isProducing = new();
        private readonly ReactiveProperty<int> _producedValue = new();

        private FolkManager _folkManager = null!;
        private Timer? _productionTimer;

        private int _folkProducing;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private float _productionTime = 3f;

        [SerializeField]
        private int _productionPerFolk = 3;

        [SerializeField]
        private string _workersTitle = "Farmers";

        public override BuildingType BuildingType => BuildingType.Farm;
        public IReadOnlyReactiveProperty<int> WorkerCount => Game.Instance.FolkManager.FarmFolk;
        public IReadOnlyReactiveProperty<int> PotentialWorkerCount => Game.Instance.FolkManager.IdleFolk;
        public int MaxWorkerCount => 0;
        public bool HasMaxWorkerCount => false;
        public string WorkersTitle => _workersTitle;

        public IReadOnlyReactiveProperty<int> ProducedValue => _producedValue;
        public IReadOnlyReactiveProperty<bool> IsProducing => _isProducing;
        public Timer? ProductionTimer => _productionTimer;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _folkManager = Game.Instance.FolkManager;
            _folkManager.FarmFolk
                .TakeUntilDisable(this)
                .Subscribe(OnFarmCountChanged);
        }

        public void AddWorker()
        {
            Game.Instance.FolkManager.AddFolkToFarm();
        }

        public void RemoveWorker()
        {
            Game.Instance.FolkManager.RemoveFolkFromFarm();
        }

        public bool CanAddWorker()
        {
            return Game.Instance.FolkManager.IdleFolk.Value > 0;
        }

        public bool CanRemoveWorker()
        {
            return Game.Instance.FolkManager.FarmFolk.Value > 0;
        }

        private void OnFarmCountChanged(int farmWorkers)
        {
            _numberOfWorkersText.text = farmWorkers.ToString();
            if (farmWorkers == 0)
            {
                StopProduction();
                return;
            }

            if (!_isProducing.Value)
            {
                StartProduction();
                return;
            }

            if (farmWorkers < _folkProducing)
            {
                _folkProducing = farmWorkers;
                _producedValue.Value = _folkProducing * _productionPerFolk;
            }
        }

        private void StartProduction()
        {
            int folkCount = _folkManager.FarmFolk.Value;
            Assert.IsTrue(folkCount > 0);
            Assert.IsTrue(_productionTimer == null);
            _productionTimer = Game.Instance.TimeSystem.CreateTimer(_productionTime, OnFoodProduced);
            _folkProducing = folkCount;
            _producedValue.Value = _folkProducing * _productionPerFolk;
            _productionTimer.Start();
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

        private void OnFoodProduced(Timer _)
        {
            Assert.IsTrue(_productionTimer != null);
            Assert.IsTrue(_isProducing.Value);
            Assert.IsTrue(_folkManager.FarmFolk.Value > 0);
            Assert.IsTrue(_folkProducing > 0);
            Game.Instance.FoodController.ProduceFoodFromFarm(_productionPerFolk * _folkProducing);
            _productionTimer = null;
            _isProducing.Value = false;
            StartProduction();
        }
    }
}