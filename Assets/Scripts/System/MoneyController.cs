#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.System
{
    public sealed class MoneyController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _governmentMoney = new();
        private readonly ReactiveProperty<int> _armyMoney = new();
        private readonly ReactiveProperty<int> _folkMoney = new();

        public IReadOnlyReactiveProperty<int> GovernmentMoney => _governmentMoney;
        public IReadOnlyReactiveProperty<int> ArmyMoney => _armyMoney;
        public IReadOnlyReactiveProperty<int> FolkMoney => _folkMoney;

        public void AddGovernmentMoney(int money)
        {
            _governmentMoney.Value += money;
        }

        public void TransferMoneyFromGovernmentToArmy(int money)
        {
            Assert.IsTrue(_governmentMoney.Value >= money);
            _governmentMoney.Value -= money;
            _armyMoney.Value += money;
        }

        public void TransferMoneyFromArmyToFolk(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            _armyMoney.Value -= money;
            _folkMoney.Value += money;
        }
    }
}