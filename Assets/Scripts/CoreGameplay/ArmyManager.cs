#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyManager : InitializableBehaviour
    {
        private const int MINIMAL_SALARY = 1;

        private readonly ReactiveProperty<int> _armyCount = new();
        private readonly ReactiveProperty<int> _guardsCount = new();
        private readonly ReactiveProperty<int> _salary = new();
        private readonly ReactiveProperty<int> _explorersCount = new();

        [SerializeField]
        private int _startingArmySalary = 5;

        [SerializeField]
        private int _startingGuardsCount = 2;

        [SerializeField]
        private int _startingExplorersCount = 0;

        public IReactiveProperty<int> ArmyCount => _armyCount;
        public IReactiveProperty<int> GuardsCount => _guardsCount;
        public IReactiveProperty<int> ExplorersCount => _explorersCount;
        public IReactiveProperty<int> Salary => _salary;

        public void InitializeGameData()
        {
            _salary.Value = _startingArmySalary;
            _guardsCount.Value = _startingGuardsCount;
            _armyCount.Value = _startingGuardsCount;
            _explorersCount.Value = _startingExplorersCount;
        }

        public void BuyArmyForFood()
        {
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int newArmyCost = coreGameplay.NewArmyForFoodCost;
            Game.Instance.FoodController.SpendArmyFood(newArmyCost);
            _guardsCount.Value++;
            _armyCount.Value++;
        }

        public void AddArmyToExplorers()
        {
            if (_guardsCount.Value == 0)
            {
                return;
            }

            _guardsCount.Value--;
            _explorersCount.Value++;
        }

        public void RemoveArmyFromExplorers()
        {
            if (_explorersCount.Value == 0)
            {
                return;
            }

            _guardsCount.Value++;
            _explorersCount.Value--;
        }

        public void RaiseSalary()
        {
            _salary.Value++;
        }

        public void LowerSalary()
        {
            if (_salary.Value <= MINIMAL_SALARY)
            {
                return;
            }

            Assert.IsTrue(_salary.Value > 1);
            _salary.Value--;
        }

        public void ReturnExplorersToGuard()
        {
            while (_explorersCount.Value > 0)
            {
                RemoveArmyFromExplorers();
            }
        }

        public void PaySalary()
        {
            DesertUnpaid();
            int armyCount = Game.Instance.Expeditions.IsExpeditionActive.Value ? _guardsCount.Value : _armyCount.Value;
            int salaryToPay = _salary.Value * armyCount;
            if (salaryToPay == 0)
            {
                return;
            }

            Assert.IsTrue(Game.Instance.MoneyController.GovernmentMoney.Value >= salaryToPay);
            Game.Instance.MoneyController.TransferMoneyFromGovernmentToArmy(salaryToPay);
        }

        public void EatFood()
        {
            KillStarved();
            int armyCount = Game.Instance.Expeditions.IsExpeditionActive.Value ? _guardsCount.Value : _armyCount.Value;
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int foodToEat = armyCount * coreGameplay.FoodPerPerson;
            Assert.IsTrue(Game.Instance.FoodController.ArmyFood.Value >= foodToEat);
            Game.Instance.FoodController.SpendArmyFood(foodToEat);
        }

        private void KillStarved()
        {
            int armyCount = Game.Instance.Expeditions.IsExpeditionActive.Value ? _guardsCount.Value : _armyCount.Value;
            FoodController foodController = Game.Instance.FoodController;
            CoreGameplay coreGameplay = Game.Instance.CoreGameplay;
            int maxArmyToFeed = foodController.ArmyFood.Value / coreGameplay.FoodPerPerson;
            int starvedArmy = armyCount - maxArmyToFeed;
            if (starvedArmy <= 0)
            {
                return;
            }

            Game.Instance.CoreGameplay.OnArmyStarved(starvedArmy);
            ReduceArmy(starvedArmy);
        }

        private void ReduceArmy(int delta)
        {
            Assert.IsTrue(_armyCount.Value >= delta);
            _armyCount.Value -= delta;
            if (_guardsCount.Value >= delta)
            {
                _guardsCount.Value -= delta;
                return;
            }

            delta -= _guardsCount.Value;
            _guardsCount.Value = 0;
            Assert.IsTrue(delta <= _explorersCount.Value);
            Assert.IsFalse(Game.Instance.Expeditions.IsExpeditionActive.Value);
            _explorersCount.Value -= delta;
        }

        private void DesertUnpaid()
        {
            int armyCount = Game.Instance.Expeditions.IsExpeditionActive.Value ? _guardsCount.Value : _armyCount.Value;
            MoneyController moneyController = Game.Instance.MoneyController;
            int governmentMoney = moneyController.GovernmentMoney.Value;
            int maxAffordableArmy = governmentMoney / _salary.Value;
            int numberToDesert = armyCount - maxAffordableArmy;
            if (numberToDesert <= 0)
            {
                return;
            }

            Game.Instance.CoreGameplay.OnArmyDeserted(numberToDesert);
            ReduceArmy(numberToDesert);
        }

        public void KillGuards(int maxKilled)
        {
            if (_guardsCount.Value == 0)
            {
                return;
            }

            int ableToKill = Mathf.Min(maxKilled, _guardsCount.Value);

            _armyCount.Value -= ableToKill;
            _guardsCount.Value -= ableToKill;
        }
    }
}