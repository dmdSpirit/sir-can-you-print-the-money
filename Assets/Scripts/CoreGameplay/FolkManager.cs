#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class FolkManager : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _folkCount = new();
        private readonly ReactiveProperty<int> _farmFolk = new();
        private readonly ReactiveProperty<int> _marketFolk = new();
        private readonly ReactiveProperty<int> _mineFolk = new();
        private readonly ReactiveProperty<int> _tax = new();

        [SerializeField]
        private int _startingFolkTax = 2;

        [SerializeField]
        private int _startingFarmWorkers = 1;

        [SerializeField]
        private int _startingMarketWorkers = 1;

        [SerializeField]
        private int _startingMineWorkers = 0;

        [SerializeField]
        private int _maxMarketWorkers = 1;

        public IReadOnlyReactiveProperty<int> FolkCount => _folkCount;
        public IReadOnlyReactiveProperty<int> FarmFolk => _farmFolk;
        public IReadOnlyReactiveProperty<int> MarketFolk => _marketFolk;
        public IReadOnlyReactiveProperty<int> MineFolk => _mineFolk;
        public IReadOnlyReactiveProperty<int> Tax => _tax;

        public int MaxMarkerWorkers => _maxMarketWorkers;

        public void InitializeGameData()
        {
            _tax.Value = _startingFolkTax;
            _farmFolk.Value = _startingFarmWorkers;
            _marketFolk.Value = _startingMarketWorkers;
            _mineFolk.Value = _startingMineWorkers;
            _folkCount.Value = _farmFolk.Value + _marketFolk.Value + _mineFolk.Value;
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
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int newFolkCost = coreGameplay.NewFolkForFoodCost;
            Assert.IsTrue(Game.Instance.FoodController.FolkFood.Value >= newFolkCost);
            Game.Instance.FoodController.SpendFolkFood(newFolkCost);
            _folkCount.Value++;
            _farmFolk.Value++;
        }

        public void AddFolkToMarket()
        {
            if (!CanAddWorkerToMarket())
            {
                return;
            }

            _farmFolk.Value--;
            _marketFolk.Value++;

            bool CanAddWorkerToMarket() => _farmFolk.Value > 0 && _marketFolk.Value < _maxMarketWorkers;
        }

        public void RemoveFolkFromMarket()
        {
            if (_marketFolk.Value <= 0)
            {
                return;
            }

            _marketFolk.Value--;
            _farmFolk.Value++;
        }

        public void AddFolkToMine()
        {
            if (!CanAddWorkerToMine())
            {
                return;
            }

            _farmFolk.Value--;
            _mineFolk.Value++;

            bool CanAddWorkerToMine() => _farmFolk.Value > 0 && Game.Instance.TechController.CanUseMine.Value;
        }

        public void RemoveFolkFromMine()
        {
            if (_mineFolk.Value <= 0)
            {
                return;
            }

            _mineFolk.Value--;
            _farmFolk.Value++;
        }

        public void RaiseTax()
        {
            _tax.Value++;
        }

        public void LowerTax()
        {
            if (_tax.Value <= 1)
            {
                return;
            }

            _tax.Value--;
        }

        private void PayTaxes()
        {
            int taxesToPay = _folkCount.Value * _tax.Value;
            if (taxesToPay <= 0)
            {
                return;
            }

            Assert.IsTrue(Game.Instance.MoneyController.FolkMoney.Value >= taxesToPay);
            Game.Instance.MoneyController.TransferMoneyFromFolkToGovernment(taxesToPay);
        }

        private void EatFood()
        {
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int foodToEat = _folkCount.Value * coreGameplay.FoodPerPerson;
            Assert.IsTrue(Game.Instance.FoodController.FolkFood.Value >= foodToEat);
            Game.Instance.FoodController.SpendFolkFood(foodToEat);
        }

        private void KillPoorAndHungry()
        {
            int starvedFolk = CalculateStarvingFolk();
            if (starvedFolk > 0)
            {
                Game.Instance.CoreGameplay.OnFolkStarved(starvedFolk);
            }

            int executedFolk = CalculatePoorFolk();
            if (executedFolk > 0)
            {
                Game.Instance.CoreGameplay.OnFolkExecuted(executedFolk);
            }

            int folkToKill = Mathf.Max(starvedFolk, executedFolk);
            if (folkToKill > 0)
            {
                KillFolk(folkToKill);
            }
        }

        private int CalculateStarvingFolk()
        {
            FoodController foodController = Game.Instance.FoodController;
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int foodPerPerson = coreGameplay.FoodPerPerson;
            int folkFood = foodController.FolkFood.Value;
            int maxFolkToEat = folkFood / foodPerPerson;
            if (_folkCount.Value <= maxFolkToEat)
            {
                return 0;
            }

            return _folkCount.Value - maxFolkToEat;
        }

        private int CalculatePoorFolk()
        {
            MoneyController moneyController = Game.Instance.MoneyController;
            int folkMoney = moneyController.FolkMoney.Value;
            int maxFolkToPay = folkMoney / _tax.Value;
            if (_folkCount.Value <= maxFolkToPay)
            {
                return 0;
            }

            return _folkCount.Value - maxFolkToPay;
        }

        public void KillFolk(int numberToExecute)
        {
            Assert.IsTrue(_folkCount.Value >= numberToExecute);
            _folkCount.Value -= numberToExecute;

            KillFolk(_farmFolk, ref numberToExecute);
            KillFolk(_marketFolk, ref numberToExecute);
            KillFolk(_mineFolk, ref numberToExecute);
            Assert.IsTrue(ValidateTotalCount());
        }

        private static void KillFolk(IReactiveProperty<int> folkCount, ref int numberToKill)
        {
            if (HasNoOneToKill(numberToKill))
            {
                return;
            }

            if (folkCount.Value >= numberToKill)
            {
                folkCount.Value -= numberToKill;
                numberToKill = 0;
                return;
            }

            numberToKill -= folkCount.Value;
            folkCount.Value = 0;

            bool HasNoOneToKill(int numberToKill) => numberToKill <= 0 || folkCount.Value == 0;
        }

        private bool ValidateTotalCount()
        {
            return _mineFolk.Value + _farmFolk.Value + _marketFolk.Value == _folkCount.Value;
        }
    }
}