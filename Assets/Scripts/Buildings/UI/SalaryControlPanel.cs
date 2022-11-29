#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class SalaryControlPanel : UIElement<ISalaryController>
    {
        private readonly CompositeDisposable _sub = new();
        private ISalaryController _salaryController = null!;

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

        [SerializeField]
        private TMP_Text _salary = null!;

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

        protected override void OnShow(ISalaryController salaryController)
        {
            _sub.Clear();
            _salaryController = salaryController;
            _salaryController.CanLowerSalary.Subscribe(UpdateLowerButtonState).AddTo(_sub);
            _salaryController.CanRaiseSalary.Subscribe(UpdateRaiseButtonState).AddTo(_sub);
            Game.Instance.ArmyManager.Salary.Subscribe(OnSalaryChanged).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _salaryController = null!;
            _sub.Clear();
        }

        private void OnSalaryChanged(int salary)
        {
            _salary.text = salary.ToString();
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
            _salaryController.RaiseSalary();
        }

        private void OnLower(Unit _)
        {
            _salaryController.LowerSalary();
        }
    }
}