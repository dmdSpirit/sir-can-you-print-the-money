#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class MoneyPrinterPanel : UIElement<IMoneyPrinter>
    {
        private readonly CompositeDisposable _sub = new();
        private IMoneyPrinter _moneyPrinter = null!;

        [SerializeField]
        private string _lockedText = "Not learned";

        [SerializeField]
        private Button _printButton = null!;

        [SerializeField]
        private TMP_Text _printButtonText = null!;

        [SerializeField]
        private string _printText = "Print";

        [SerializeField]
        private Button _burnButton = null!;

        [SerializeField]
        private TMP_Text _burnButtonText = null!;

        [SerializeField]
        private string _burnText = "Burn";

        [SerializeField]
        private TMP_Text _moneyText = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _printButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnPrint);
            _burnButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnBurn);
        }

        protected override void OnShow(IMoneyPrinter moneyPrinter)
        {
            _sub.Clear();
            _moneyPrinter = moneyPrinter;
            _moneyPrinter.CanPrintMoney.Subscribe(UpdatePrintButtonState).AddTo(_sub);
            _moneyPrinter.CanBurnMoney.Subscribe(UpdateBurnButtonState).AddTo(_sub);
            Game.Instance.MoneyController.GovernmentMoney.Subscribe(OnMoneyChanged).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _moneyPrinter = null!;
            _sub.Clear();
        }

        private void OnMoneyChanged(int salary)
        {
            _moneyText.text = salary.ToString();
        }

        private void UpdatePrintButtonState(bool canPrint)
        {
            _printButton.interactable = canPrint;
            _printButtonText.text = canPrint ? _printText : _lockedText;
        }

        private void UpdateBurnButtonState(bool canBurn)
        {
            _burnButton.interactable = canBurn;
            _burnButtonText.text = canBurn ? _burnText : _lockedText;
        }

        private void OnPrint(Unit _)
        {
            _moneyPrinter.PrintMoney();
        }

        private void OnBurn(Unit _)
        {
            _moneyPrinter.BurnMoney();
        }
    }
}