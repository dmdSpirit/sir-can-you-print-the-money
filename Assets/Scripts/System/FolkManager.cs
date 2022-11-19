#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.System
{
    public sealed class FolkManager : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _folkCount = new();
        private readonly ReactiveProperty<int> _foodCount = new();
        private readonly ReactiveProperty<int> _farmFolk = new();
        private readonly ReactiveProperty<int> _idleFolk = new();
        private readonly ReactiveProperty<int> _marketFolk = new();

        private int _tax;

        [SerializeField]
        private int _folkTax = 2;

        [SerializeField]
        private int _startingFolkCount = 2;

        public IReadOnlyReactiveProperty<int> FolkCount => _folkCount;
        public IReadOnlyReactiveProperty<int> FoodCount => _foodCount;
        public IReadOnlyReactiveProperty<int> FarmFolk => _farmFolk;
        public IReadOnlyReactiveProperty<int> IdleFolk => _idleFolk;
        public IReadOnlyReactiveProperty<int> MarketFolk => _marketFolk;

        public void StartRound()
        {
        }

        public void EndRound()
        {
            KillPoorAndHungry();
            PayTaxes();
            EatFood();
        }

        public void SetTax(int tax) => _tax = tax;

        public void BuyFolkForFood()
        {
            int newFolkCost = Game.Instance.CoreGameplay.NewFolkForFoodCost;
            Assert.IsTrue(_foodCount.Value >= newFolkCost);
            _foodCount.Value -= newFolkCost;
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
            int taxesToPay = _folkCount.Value * _tax;
            Assert.IsTrue(Game.Instance.MoneyController.FolkMoney.Value >= taxesToPay);
            Game.Instance.MoneyController.TransferMoneyFromFolkToGovernment(taxesToPay);
        }

        private void EatFood()
        {
            int foodToEat = _folkCount.Value * Game.Instance.CoreGameplay.FoodPerPerson;
            Assert.IsTrue(_foodCount.Value >= foodToEat);
            _foodCount.Value -= foodToEat;
        }

        private void KillPoorAndHungry()
        {
            int folkMoney = Game.Instance.MoneyController.FolkMoney.Value;
            int foodPerPerson = Game.Instance.CoreGameplay.FoodPerPerson;
            int maxFolkToFeed = _foodCount.Value / foodPerPerson;
            int maxFolkToAffordTax = folkMoney / _tax;
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
        }
    }
}