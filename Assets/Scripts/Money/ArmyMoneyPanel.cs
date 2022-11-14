#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Money
{
    public sealed class ArmyMoneyPanel : InitializableBehaviour
    {
        [SerializeField]
        private TMP_Text _moneyText = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Game.Instance.MoneyController.ArmyMoney
                .TakeUntilDisable(this)
                .Subscribe(UpdateMoney);
        }

        private void UpdateMoney(int money) => _moneyText.text = money.ToString();
    }
}