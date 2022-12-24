#nullable enable
using NovemberProject.Core;
using NovemberProject.Core.FolkManagement;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class FarmBuilding : Building, IProducer, IResourceStorage
    {
        private readonly ReactiveProperty<bool> _isProducing = new();
        private readonly ReactiveProperty<int> _producedValue = new();

        private FolkManager _folkManager = null!;
        private FoodController _foodController = null!;
        private TimeSystem _timeSystem = null!;
        private Timer? _productionTimer;

        private int _folkProducing;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private float _productionTime = 3f;

        [SerializeField]
        private int _productionPerFolk = 3;

        [SerializeField]
        private Sprite _farmerImage = null!;

        [SerializeField]
        private string _farmerTitle = "Farmers";

        public override BuildingType BuildingType => BuildingType.Farm;

        public IReadOnlyReactiveProperty<int> ProducedValue => _producedValue;
        public IReadOnlyReactiveProperty<bool> IsProducing => _isProducing;
        public IReadOnlyTimer? ProductionTimer => _productionTimer;
        public bool ShowProducedValue => true;
        public Sprite SpriteIcon => _farmerImage;
        public IReadOnlyReactiveProperty<int> ResourceCount => _folkManager.FarmFolk;
        public string ResourceTitle => _farmerTitle;

        [Inject]
        private void Construct(FolkManager folkManager, FoodController foodController, TimeSystem timeSystem)
        {
            _folkManager = folkManager;
            _foodController = foodController;
            _timeSystem = timeSystem;
            _folkManager.FarmFolk.Subscribe(OnFarmCountChanged);
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
            _productionTimer = _timeSystem.CreateTimer(_productionTime, OnFoodProduced);
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
            _foodController.ProduceFoodFromFarm(_productionPerFolk * _folkProducing);
            _productionTimer = null;
            _isProducing.Value = false;
            StartProduction();
        }
    }
}