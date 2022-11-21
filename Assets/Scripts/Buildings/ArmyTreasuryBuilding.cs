#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ArmyTreasuryBuilding : Building
    {
        private MoneyController _moneyController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        public override BuildingType BuildingType => BuildingType.ArmyTreasury;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _moneyController = Game.Instance.MoneyController;
            _moneyController.ArmyMoney
                .TakeUntilDisable(this)
                .Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}