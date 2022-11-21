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
    public sealed class FarmBuilding : Building
    {
        private FolkManager _folkManager = null!;
        private Timer? _productionTimer;
        private bool _isProducing;

        private int _folkProducing;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private float _productionTime = 3f;

        [SerializeField]
        private int _productionPerFolk = 3;

        public override BuildingType BuildingType => BuildingType.Farm;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _folkManager = Game.Instance.FolkManager;
            _folkManager.FarmFolk
                .TakeUntilDisable(this)
                .Subscribe(OnFarmCountChanged);
        }

        private void OnFarmCountChanged(int farmWorkers)
        {
            _numberOfWorkersText.text = farmWorkers.ToString();
            if (farmWorkers == 0)
            {
                StopProduction();
                return;
            }

            if (!_isProducing)
            {
                StartProduction();
                return;
            }

            if (farmWorkers < _folkProducing)
            {
                _folkProducing = farmWorkers;
            }
        }

        private void StartProduction()
        {
            int folkCount = _folkManager.FarmFolk.Value;
            Assert.IsTrue(folkCount > 0);
            Assert.IsTrue(_productionTimer == null);
            _productionTimer = Game.Instance.TimeSystem.CreateTimer(_productionTime, OnFoodProduced);
            _folkProducing = folkCount;
            _productionTimer.Start();
            _isProducing = true;
        }

        private void StopProduction()
        {
            if (!_isProducing)
            {
                return;
            }

            _productionTimer?.Cancel();
            _productionTimer = null;
            _isProducing = false;
        }

        private void OnFoodProduced(Timer _)
        {
            Assert.IsTrue(_productionTimer != null);
            Assert.IsTrue(_isProducing);
            Assert.IsTrue(_folkManager.FarmFolk.Value > 0);
            Assert.IsTrue(_folkProducing > 0);
            Game.Instance.FoodController.ProduceFoodFromFarm(_productionPerFolk * _folkProducing);
            _productionTimer = null;
            _isProducing = false;
            StartProduction();
        }
    }
}