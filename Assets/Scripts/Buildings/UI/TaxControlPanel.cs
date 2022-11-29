#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class TaxControlPanel : UIElement<ITaxController>
    {
        private readonly CompositeDisposable _sub = new();
        private ITaxController _taxController = null!;

        [SerializeField]
        private TMP_Text _tax = null!;

        [SerializeField]
        private string _lockedText = "Not learned";

        [SerializeField]
        private Button _raiseButton = null!;

        [SerializeField]
        private TMP_Text _raiseButtonText = null!;

        [SerializeField]
        private string _raiseText = "Raise";

        [SerializeField]
        private Button _lowerButton = null!;

        [SerializeField]
        private TMP_Text _lowerButtonText = null!;

        [SerializeField]
        private string _lowerText = "Lower";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _raiseButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnRaise);
            _lowerButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnLower);
        }

        protected override void OnShow(ITaxController salaryController)
        {
            _sub.Clear();
            _taxController = salaryController;
            _taxController.CanLowerTax.Subscribe(UpdateLowerButtonState).AddTo(_sub);
            _taxController.CanRaiseTax.Subscribe(UpdateRaiseButtonState).AddTo(_sub);
            Game.Instance.FolkManager.Tax.Subscribe(OnTaxChanged).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _taxController = null!;
            _sub.Clear();
        }

        private void OnTaxChanged(int tax)
        {
            _tax.text = tax.ToString();
        }

        private void UpdateRaiseButtonState(bool canRaise)
        {
            _raiseButton.interactable = canRaise;
            _raiseButtonText.text = canRaise ? _raiseText : _lockedText;
        }

        private void UpdateLowerButtonState(bool canLower)
        {
            _lowerButton.interactable = canLower;
            _lowerButtonText.text = canLower ? _lowerText : _lockedText;
        }

        private void OnRaise(Unit _)
        {
            _taxController.RaiseTax();
        }

        private void OnLower(Unit _)
        {
            _taxController.LowerTax();
        }
    }
}