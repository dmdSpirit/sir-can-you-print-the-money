#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay.Messages;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyManager : InitializableBehaviour
    {
        private const int MINIMAL_SALARY = 1;

        private readonly ReactiveProperty<int> _armyCount = new();
        private readonly ReactiveProperty<int> _salary = new();

        [SerializeField]
        private int _startingArmySalary = 5;

        [SerializeField]
        private int _startingArmyCount = 2;

        public IReactiveProperty<int> ArmyCount => _armyCount;
        public IReactiveProperty<int> Salary => _salary;

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

        public void BuyArmyForFood()
        {
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int newArmyCost = coreGameplay.NewArmyForFoodCost;
            Game.Instance.FoodController.SpendArmyFood(newArmyCost);
            _armyCount.Value++;
        }

        public void RaiseSalary()
        {
            _salary.Value++;
        }

        public void LowerSalary()
        {
            if (_salary.Value <= MINIMAL_SALARY)
            {
                Debug.LogWarning($"Can not lower salary less than {MINIMAL_SALARY}");
                return;
            }

            Assert.IsTrue(_salary.Value > 1);
            _salary.Value--;
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
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int foodToEat = _armyCount.Value * coreGameplay.FoodPerPerson;
            Assert.IsTrue(Game.Instance.FoodController.ArmyFood.Value >= foodToEat);
            Game.Instance.FoodController.SpendArmyFood(foodToEat);
        }

        private void KillStarved()
        {
            FoodController foodController = Game.Instance.FoodController;
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int maxArmyToFeed = foodController.ArmyFood.Value / coreGameplay.FoodPerPerson;
            int starvedArmy = _armyCount.Value - maxArmyToFeed;
            if (starvedArmy <= 0)
            {
                return;
            }

            Game.PublishMessage(new ArmyStarvedMessage(starvedArmy));
            ReduceArmy(starvedArmy);
        }

        private void ReduceArmy(int delta)
        {
            Assert.IsTrue(_armyCount.Value >= delta);
            _armyCount.Value -= delta;
        }

        private void DesertUnpaid()
        {
            MoneyController moneyController = Game.Instance.MoneyController;
            int governmentMoney = moneyController.GovernmentMoney.Value;
            int maxAffordableArmy = governmentMoney / _salary.Value;
            int numberToDesert = _armyCount.Value - maxAffordableArmy;
            if (numberToDesert <= 0)
            {
                return;
            }

            Game.PublishMessage(new ArmyDesertedMessage(numberToDesert));
            ReduceArmy(numberToDesert);
        }
    }
}