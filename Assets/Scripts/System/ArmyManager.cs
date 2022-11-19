#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.System
{
    public sealed class ArmyManager : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _armyCount = new();
        private readonly ReactiveProperty<int> _foodCount = new();
        private readonly ReactiveProperty<int> _salary = new();

        [SerializeField]
        private int _startingArmySalary = 5;

        [SerializeField]
        private int _startingArmyCount = 2;

        public IReactiveProperty<int> ArmyCount => _armyCount;
        public IReactiveProperty<int> Salary => _salary;
        private IReactiveProperty<int> FoodCount => _foodCount;
        
        
        public void InitializeGameData()
        {
            _salary.Value = _startingArmySalary;
            _armyCount.Value = _startingArmyCount;
        }

        public void StartRound()
        {
            DesertUnpaid();
            PaySalary();
        }

        public void EndRound()
        {
            KillStarved();
            EatFood();
        }

        public void SetSalary(int salary) => _salary.Value = salary;

        public void BuyArmyForFood()
        {
            int newArmyCost = Game.Instance.CoreGameplay.NewArmyForFoodCost;
            Assert.IsTrue(_foodCount.Value >= newArmyCost);
            _foodCount.Value -= newArmyCost;
            _armyCount.Value++;
        }

        private void PaySalary()
        {
            int salaryToPay = _salary.Value * _armyCount.Value;
            if (salaryToPay == 0)
            {
                return;
            }

            Assert.IsTrue(Game.Instance.MoneyController.GovernmentMoney.Value >= salaryToPay);
            Game.Instance.MoneyController.TransferMoneyFromGovernmentToArmy(salaryToPay);
        }

        private void EatFood()
        {
            int foodToEat = _armyCount.Value * Game.Instance.CoreGameplay.FoodPerPerson;
            Assert.IsTrue(_foodCount.Value >= foodToEat);
            _foodCount.Value -= foodToEat;
        }

        private void KillStarved()
        {
            int maxArmyToFeed = _foodCount.Value / Game.Instance.CoreGameplay.FoodPerPerson;
            int starvedArmy = _armyCount.Value - maxArmyToFeed;
            if (starvedArmy > 0)
            {
                Game.PublishMessage(new ArmyStarvedMessage(starvedArmy));
                ReduceArmy(starvedArmy);
            }
        }

        private void DesertUnpaid()
        {
            int governmentMoney = Game.Instance.MoneyController.GovernmentMoney.Value;
            int maxAffordableArmy = governmentMoney / _salary.Value;
            int numberToDesert = _armyCount.Value - maxAffordableArmy;
            if (numberToDesert > 0)
            {
                Game.PublishMessage(new ArmyDesertedMessage(numberToDesert));
                ReduceArmy(numberToDesert);
            }
        }

        private void ReduceArmy(int delta)
        {
            Assert.IsTrue(_armyCount.Value >= delta);
            _armyCount.Value -= delta;
        }
    }
}