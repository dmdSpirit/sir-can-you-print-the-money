#nullable enable
using NovemberProject.System.Messages;
using NovemberProject.TechTree;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.Core.FolkManagement
{
    public sealed class FolkManager : IUnitManager
    {
        private readonly ReactiveProperty<int> _folkCount = new();
        private readonly ReactiveProperty<int> _farmFolk = new();
        private readonly ReactiveProperty<int> _marketFolk = new();
        private readonly ReactiveProperty<int> _mineFolk = new();
        private readonly ReactiveProperty<int> _tax = new();

        private readonly FolkManagerSettings _settings;
        private readonly FoodController _foodController;
        private readonly TechController _techController;
        private readonly MoneyController _moneyController;
        private readonly MessageBroker _messageBroker;

        public IReadOnlyReactiveProperty<int> FolkCount => _folkCount;
        public IReadOnlyReactiveProperty<int> FarmFolk => _farmFolk;
        public IReadOnlyReactiveProperty<int> MarketFolk => _marketFolk;
        public IReadOnlyReactiveProperty<int> MineFolk => _mineFolk;
        public IReadOnlyReactiveProperty<int> Tax => _tax;

        public int MaxMarkerWorkers => _settings.MaxMarketWorkers;

        public FolkManager(FolkManagerSettings settings, FoodController foodController,
            TechController techController, MoneyController moneyController,
            MessageBroker messageBroker)
        {
            _settings = settings;
            _foodController = foodController;
            _techController = techController;
            _moneyController = moneyController;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _tax.Value = _settings.FolkTax;
            _farmFolk.Value = _settings.FarmWorkers;
            _marketFolk.Value = _settings.MarketWorkers;
            _mineFolk.Value = _settings.MineWorkers;
            // TODO (Stas): refactor folk counter into some kind of container 
            _folkCount.Value = _farmFolk.Value + _marketFolk.Value + _mineFolk.Value;
        }

        public void BuyUnit()
        {
            int newFolkCost = _settings.NewUnitFoodCost;
            _foodController.SpendFolkFood(newFolkCost);
            SpawnFolk();
        }

        private void SpawnFolk()
        {
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
        }

        private bool CanAddWorkerToMarket() => _farmFolk.Value > 0 && _marketFolk.Value < _settings.MaxMarketWorkers;

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
        }

        public bool CanAddWorkerToMine() => _farmFolk.Value > 0 && _techController.CanUseMine.Value;

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

        public void PayTaxes()
        {
            KillPoor();
            int taxesToPay = _folkCount.Value * _tax.Value;
            if (taxesToPay <= 0)
            {
                return;
            }

            _moneyController.TransferMoneyFromFolkToGovernment(taxesToPay);
        }

        public bool IsNoFolkLeft() =>
            _folkCount.Value == 0 && _foodController.FolkFood.Value < _settings.NewUnitFoodCost;

        public void EatFood()
        {
            KillHungry();
            int foodToEat = _folkCount.Value * _settings.FoodUpkeep;
            _foodController.SpendFolkFood(foodToEat);
        }

        private void KillHungry()
        {
            int starvedFolk = CalculateStarvingFolk();
            if (starvedFolk <= 0)
            {
                return;
            }

            // TODO (Stas): Turn into event for notification system and week-end logger
            _messageBroker.Publish(new FolkStarvedMessage(starvedFolk));
            KillFolk(starvedFolk);
        }

        private void KillPoor()
        {
            int executedFolk = CalculatePoorFolk();
            if (executedFolk <= 0)
            {
                return;
            }

            // TODO (Stas): Turn into event for notification system and week-end logger
            _messageBroker.Publish(new FolkExecutedMessage(executedFolk));
            KillFolk(executedFolk);
        }

        private int CalculateStarvingFolk()
        {
            int foodPerPerson = _settings.FoodUpkeep;
            int folkFood = _foodController.FolkFood.Value;
            int maxFolkToEat = folkFood / foodPerPerson;
            if (_folkCount.Value <= maxFolkToEat)
            {
                return 0;
            }

            return _folkCount.Value - maxFolkToEat;
        }

        private int CalculatePoorFolk()
        {
            int folkMoney = _moneyController.FolkMoney.Value;
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

        public bool CanBuyUnit() => _foodController.FolkFood.Value >= _settings.NewUnitFoodCost;

        private bool ValidateTotalCount()
        {
            return _mineFolk.Value + _farmFolk.Value + _marketFolk.Value == _folkCount.Value;
        }

        public bool CanAddMarketWorker() => _farmFolk.Value > 0 && _marketFolk.Value < _settings.MaxMarketWorkers;

        public bool CanRemoveMarketWorker() => _marketFolk.Value > 0;

        public bool CanRemoveMineWorker() => _mineFolk.Value > 0;
    }
}