#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.System
{
    public sealed class FolkManager : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _folkCount = new();
        private readonly ReactiveProperty<int> _farmFolk = new();
        private readonly ReactiveProperty<int> _idleFolk = new();
        private readonly ReactiveProperty<int> _marketFolk = new();
        private readonly ReactiveProperty<int> _tax = new();

        [SerializeField]
        private int _startingFolkTax = 2;

        [SerializeField]
        private int _startingFolkCount = 3;

        [SerializeField]
        private int _startingFarmWorkers = 1;

        [SerializeField]
        private int _startingMarketWorkers = 1;

        public IReadOnlyReactiveProperty<int> FolkCount => _folkCount;
        public IReadOnlyReactiveProperty<int> FarmFolk => _farmFolk;
        public IReadOnlyReactiveProperty<int> IdleFolk => _idleFolk;
        public IReadOnlyReactiveProperty<int> MarketFolk => _marketFolk;
        public IReadOnlyReactiveProperty<int> Tax => _tax;

        public void InitializeGameData()
        {
            _folkCount.Value = _startingFolkCount;
            _idleFolk.Value = _startingFolkCount - _startingFarmWorkers - _startingMarketWorkers;
            _tax.Value = _startingFolkTax;
            _farmFolk.Value = _startingFarmWorkers;
            _marketFolk.Value = _startingMarketWorkers;
        }

        public void StartRound()
        {
        }

        public void EndRound()
        {
            KillPoorAndHungry();
            PayTaxes();
            EatFood();
        }

        public void BuyFolkForFood()
        {
            int newFolkCost = Game.Instance.CoreGameplay.NewFolkForFoodCost;
            Assert.IsTrue(Game.Instance.FoodController.FolkFood.Value >= newFolkCost);
            Game.Instance.FoodController.SpendFolkFood(newFolkCost);
            _folkCount.Value++;
            _idleFolk.Value++;
        }

        public void AddFolkToFarm()
        {
            Assert.IsTrue(_idleFolk.Value >= 0);
            _idleFolk.Value--;
            _farmFolk.Value++;
        }

        public void RemoveFolkFromFarm()
        {
            Assert.IsTrue(_farmFolk.Value >= 0);
            _farmFolk.Value--;
            _idleFolk.Value++;
        }

        public void AddFolkToMarket()
        {
            Assert.IsTrue(_idleFolk.Value >= 0);
            Assert.IsTrue(_marketFolk.Value == 0);
            _idleFolk.Value--;
            _marketFolk.Value++;
        }

        public void RemoveFolkFromMarket()
        {
            Assert.IsTrue(_marketFolk.Value >= 0);
            _marketFolk.Value--;
            _idleFolk.Value++;
        }

        private void PayTaxes()
        {
            int taxesToPay = _folkCount.Value * _tax.Value;
            if (taxesToPay == 0)
            {
                return;
            }

            Assert.IsTrue(Game.Instance.MoneyController.FolkMoney.Value >= taxesToPay);
            Game.Instance.MoneyController.TransferMoneyFromFolkToGovernment(taxesToPay);
        }

        private void EatFood()
        {
            int foodToEat = _folkCount.Value * Game.Instance.CoreGameplay.FoodPerPerson;
            Assert.IsTrue(Game.Instance.FoodController.FolkFood.Value >= foodToEat);
            Game.Instance.FoodController.SpendFolkFood(foodToEat);
        }

        private void KillPoorAndHungry()
        {
            int folkMoney = Game.Instance.MoneyController.FolkMoney.Value;
            int foodPerPerson = Game.Instance.CoreGameplay.FoodPerPerson;
            int maxFolkToFeed = Game.Instance.FoodController.FolkFood.Value / foodPerPerson;
            int maxFolkToAffordTax = folkMoney / _tax.Value;
            int maxFolkToLive = Mathf.Min(maxFolkToFeed, maxFolkToAffordTax, _folkCount.Value);
            int starvedFolk = _folkCount.Value - maxFolkToFeed;
            if (starvedFolk > 0)
            {
                Game.PublishMessage(new FolkStarvedMessage(starvedFolk));
            }

            int executedFolk = _folkCount.Value - maxFolkToAffordTax;
            if (executedFolk > 0)
            {
                Game.PublishMessage(new FolkExecutedMessage(executedFolk));
            }

            int folkToKill = _folkCount.Value - maxFolkToLive;
            if (folkToKill > 0)
            {
                KillFolk(folkToKill);
            }
        }

        private void KillFolk(int numberToExecute)
        {
            Assert.IsTrue(_folkCount.Value >= numberToExecute);
            _folkCount.Value -= numberToExecute;
            if (_idleFolk.Value > 0)
            {
                if (_idleFolk.Value >= numberToExecute)
                {
                    _idleFolk.Value -= numberToExecute;
                    Assert.IsTrue(ValidateTotalCount());
                    return;
                }

                numberToExecute -= _idleFolk.Value;
                _idleFolk.Value = 0;
            }

            if (_farmFolk.Value > 0)
            {
                if (_farmFolk.Value >= numberToExecute)
                {
                    _farmFolk.Value -= numberToExecute;
                    Assert.IsTrue(ValidateTotalCount());
                    return;
                }
            }

            _marketFolk.Value -= numberToExecute;
            Assert.IsTrue(ValidateTotalCount());
        }

        private bool ValidateTotalCount()
        {
            return _idleFolk.Value + _farmFolk.Value + _marketFolk.Value == _folkCount.Value;
        }

        public void RaiseTax()
        {
            _tax.Value++;
        }

        public void LowerTax()
        {
            Assert.IsTrue(_tax.Value > 1);
            _tax.Value--;
        }
    }
}