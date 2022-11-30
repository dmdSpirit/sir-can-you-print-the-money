#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.MovingResources;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.CoreGameplay
{
    public sealed class MoneyController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _governmentMoney = new();
        private readonly ReactiveProperty<int> _armyMoney = new();
        private readonly ReactiveProperty<int> _folkMoney = new();

        [SerializeField]
        private int _startingGovernmentMoney = 100;

        [SerializeField]
        private int _startingArmyMoney = 0;

        [SerializeField]
        private int _startingFolkMoney = 13;

        public IReadOnlyReactiveProperty<int> GovernmentMoney => _governmentMoney;
        public IReadOnlyReactiveProperty<int> ArmyMoney => _armyMoney;
        public IReadOnlyReactiveProperty<int> FolkMoney => _folkMoney;

        public void InitializeGameData()
        {
            _governmentMoney.Value = _startingGovernmentMoney;
            _folkMoney.Value = _startingFolkMoney;
            _armyMoney.Value = _startingArmyMoney;
        }

        public void PrintMoney(int money)
        {
            _governmentMoney.Value += money;
        }

        public void BurnMoney(int money)
        {
            int moneyToBurn = Mathf.Min(money, _governmentMoney.Value);
            _governmentMoney.Value -= moneyToBurn;
        }

        public void TransferMoneyFromGovernmentToArmySilent(int money)
        {
            Assert.IsTrue(_governmentMoney.Value >= money);
            _governmentMoney.Value -= money;
            _armyMoney.Value += money;
        }

        public void TransferMoneyFromGovernmentToArmy(int money)
        {
            Assert.IsTrue(_governmentMoney.Value >= money);
            BuildingsController buildingController = Game.Instance.BuildingsController;
            Building governmentTreasury = buildingController.GetBuilding(BuildingType.GovernmentTreasury);
            Building armyTreasury = buildingController.GetBuilding(BuildingType.ArmyTreasury);
            _governmentMoney.Value -= money;
            ShowCoinMove(governmentTreasury.transform, armyTreasury.transform,
                () => _armyMoney.Value += money);
        }

        public void TransferMoneyFromArmyToFolkSilent(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            _armyMoney.Value -= money;
            _folkMoney.Value += money;
        }

        public void TransferMoneyFromArmyToFolk(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            BuildingsController buildingController = Game.Instance.BuildingsController;
            Building armyTreasury = buildingController.GetBuilding(BuildingType.ArmyTreasury);
            Building folkTreasury = buildingController.GetBuilding(BuildingType.FolkTreasury);
            _armyMoney.Value -= money;
            ShowCoinMove(armyTreasury.transform, folkTreasury.transform, () => _folkMoney.Value += money);
        }

        public void TransferMoneyFromFolkToGovernmentSilent(int money)
        {
            Assert.IsTrue(_folkMoney.Value >= money);
            _governmentMoney.Value += money;
            _folkMoney.Value -= money;
        }

        public void TransferMoneyFromFolkToGovernment(int money)
        {
            Assert.IsTrue(_folkMoney.Value >= money);
            BuildingsController buildingController = Game.Instance.BuildingsController;
            Building folkTreasury = buildingController.GetBuilding(BuildingType.FolkTreasury);
            Building governmentTreasury = buildingController.GetBuilding(BuildingType.GovernmentTreasury);
            _folkMoney.Value -= money;
            ShowCoinMove(folkTreasury.transform, governmentTreasury.transform,
                () => _governmentMoney.Value += money);
        }

        public void SpedArmyMoney(int money)
        {
            Assert.IsTrue(_armyMoney.Value >= money);
            _armyMoney.Value -= money;
        }

        private void ShowCoinMove(Transform start, Transform finish, Action callback)
        {
            ResourceMoveEffectSpawner moveEffectSpawner = Game.Instance.ResourceMoveEffectSpawner;
            MoveEffect effect = moveEffectSpawner.ShowMovingCoin(start.position, finish.position);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}