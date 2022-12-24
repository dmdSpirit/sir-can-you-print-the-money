#nullable enable
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Core
{
    public sealed class ArmyManager : IUnitManager
    {
        private const int MINIMAL_SALARY = 1;

        private readonly ReactiveProperty<int> _armyCount = new();
        private readonly ReactiveProperty<int> _guardsCount = new();
        private readonly ReactiveProperty<int> _salary = new();
        private readonly ReactiveProperty<int> _explorersCount = new();

        private readonly ArmyManagerSettings _settings;
        private readonly FoodController _foodController;
        private readonly MoneyController _moneyController;
        private readonly MessageBroker _messageBroker;

        private bool _explorersLeftToExpedition;

        public IReactiveProperty<int> ArmyCount => _armyCount;
        public IReactiveProperty<int> GuardsCount => _guardsCount;
        public IReactiveProperty<int> ExplorersCount => _explorersCount;
        public IReactiveProperty<int> Salary => _salary;

        public ArmyManager(ArmyManagerSettings armyManagerSettings, FoodController foodController,
            MoneyController moneyController,
            MessageBroker messageBroker)
        {
            _settings = armyManagerSettings;
            _foodController = foodController;
            _moneyController = moneyController;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _salary.Value = _settings.ArmySalary;
            _guardsCount.Value = _settings.GuardsCount;
            _armyCount.Value = _settings.GuardsCount;
            _explorersCount.Value = _settings.ExplorersCount;
            _explorersLeftToExpedition = false;
        }

        public bool CanBuyUnit() => _foodController.ArmyFood.Value >= _settings.NewUnitFoodCost;

        public void BuyUnit()
        {
            _foodController.SpendArmyFood(_settings.NewUnitFoodCost);
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

            _explorersLeftToExpedition = false;
        }

        public void PaySalary()
        {
            DesertUnpaid();
            int armyCount = _explorersLeftToExpedition ? _guardsCount.Value : _armyCount.Value;
            int salaryToPay = _salary.Value * armyCount;
            if (salaryToPay == 0)
            {
                return;
            }

            _moneyController.TransferMoneyFromGovernmentToArmy(salaryToPay);
        }

        public void EatFood()
        {
            KillStarved();
            int armyCount = _explorersLeftToExpedition ? _guardsCount.Value : _armyCount.Value;
            int foodToEat = armyCount * _settings.FoodUpkeep;
            _foodController.SpendArmyFood(foodToEat);
        }

        private void KillStarved()
        {
            int armyCount = _explorersLeftToExpedition ? _guardsCount.Value : _armyCount.Value;
            int maxArmyToFeed = _foodController.ArmyFood.Value / _settings.FoodUpkeep;
            int starvedArmy = armyCount - maxArmyToFeed;
            if (starvedArmy <= 0)
            {
                return;
            }

            // TODO (Stas): Turn into event for notification system and week-end logger
            _messageBroker.Publish(new ArmyStarvedMessage(starvedArmy));
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
            Assert.IsFalse(_explorersLeftToExpedition);
            _explorersCount.Value -= delta;
        }

        private void DesertUnpaid()
        {
            int armyCount = _explorersLeftToExpedition ? _guardsCount.Value : _armyCount.Value;
            int governmentMoney = _moneyController.GovernmentMoney.Value;
            int maxAffordableArmy = governmentMoney / _salary.Value;
            int numberToDesert = armyCount - maxAffordableArmy;
            if (numberToDesert <= 0)
            {
                return;
            }

            // TODO (Stas): Turn into event for notification system and week-end logger
            _messageBroker.Publish(new ArmyDesertedMessage(numberToDesert));
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

        public bool IsNoArmyLeft() =>
            _armyCount.Value == 0 && _foodController.ArmyFood.Value < _settings.NewUnitFoodCost;

        public void OnExpeditionStart()
        {
            _explorersLeftToExpedition = true;
        }
    }
}