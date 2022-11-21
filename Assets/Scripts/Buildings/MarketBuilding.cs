#nullable enable
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Buildings
{
    public sealed class MarketBuilding : Building
    {
        private Timer? _productionTimer;
        private bool _isProducing;

        [SerializeField]
        private TMP_Text _workerText = null!;

        [SerializeField]
        private float _tradeDuration = 5f;

        [SerializeField]
        private int _foodCostPerUnit = 2;

        [SerializeField]
        private int _foodChangedPerTrade = 1;

        public override BuildingType BuildingType => BuildingType.Market;

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

        private void UpdateProduction()
        {
            _workerText.text = Game.Instance.FolkManager.MarketFolk.Value.ToString();
            if (_isProducing && !IsAbleToTrade())
            {
                StopProduction();
                return;
            }

            if (!_isProducing && IsAbleToTrade())
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

        private void OnTradeFinished(Timer _)
        {
            Assert.IsTrue(_productionTimer != null);
            Assert.IsTrue(_isProducing);
            Assert.IsTrue(Game.Instance.FolkManager.MarketFolk.Value > 0);
            Game.Instance.MoneyController.TransferMoneyFromArmyToFolk(_foodCostPerUnit * _foodChangedPerTrade);
            Game.Instance.FoodController.TransferFoodFromFolkToArmy(_foodChangedPerTrade);
            _productionTimer = null;
            _isProducing = false;
            StartProduction();
        }
    }
}