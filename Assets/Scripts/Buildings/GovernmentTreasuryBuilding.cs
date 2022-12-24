#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.Core;
using NovemberProject.TechTree;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class GovernmentTreasuryBuilding : MonoBehaviour, IMoneyPrinter
    {
        private MoneyController _moneyController = null!;
        private TechController _techController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private int _moneyToPrint = 10;

        public IReadOnlyReactiveProperty<bool> CanPrintMoney => _techController.CanPrintMoney;
        public IReadOnlyReactiveProperty<bool> CanBurnMoney => _techController.CanBurnMoney;

        [Inject]
        private void Construct(MoneyController moneyController, TechController techController)
        {
            _moneyController = moneyController;
            _techController = techController;
            _moneyController.GovernmentMoney.Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }

        public void PrintMoney() => _moneyController.PrintMoney(_moneyToPrint);
        public void BurnMoney() => _moneyController.BurnMoney(_moneyToPrint);
    }
}