#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Money
{
    public sealed class MoneyController : InitializableBehaviour
    {
        private readonly ReactiveProperty<int> _governmentMoney = new();
        private readonly ReactiveProperty<int> _armyMoney = new();
        private readonly ReactiveProperty<int> _folkMoney = new();

        [SerializeField]
        private float _height = 1f;

        [SerializeField]
        private float _duration = 1f;

        public IReadOnlyReactiveProperty<int> GovernmentMoney => _governmentMoney;
        public IReadOnlyReactiveProperty<int> ArmyMoney => _armyMoney;
        public IReadOnlyReactiveProperty<int> FolkMoney => _folkMoney;

        public void AddGovernmentMoney(int money)
        {
            _governmentMoney.Value += money;
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
            ShowCoinMove(governmentTreasury.transform, armyTreasury.transform, _duration, () => _armyMoney.Value += money);
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
            ShowCoinMove(armyTreasury.transform, folkTreasury.transform, _duration, () => _folkMoney.Value += money);
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
            ShowCoinMove(folkTreasury.transform, governmentTreasury.transform, _duration,
                () => _governmentMoney.Value += money);
        }

        private void ShowCoinMove(Transform start, Transform finish, float duration, Action callback)
        {
            ResourceMoveEffectSpawner moveEffectSpawner = Game.Instance.ResourceMoveEffectSpawner;
            Vector3 startPosition = start.position;
            startPosition.y += _height;
            Vector3 finishPosition = finish.position;
            finishPosition.y += _height;
            MoveEffect effect = moveEffectSpawner.ShowMovingCoin(startPosition, finishPosition, duration);
            effect.OnFinished.Subscribe(_ => callback.Invoke());
        }
    }
}