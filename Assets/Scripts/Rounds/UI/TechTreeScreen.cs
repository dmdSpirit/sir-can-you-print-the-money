#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class TechTreeScreen : UIElement<object?>
    {
        private readonly CompositeDisposable _sub = new();

        [SerializeField]
        private TMP_Text _treasuresCount = null!;

        [SerializeField]
        private Button _closeButton = null!;

        [SerializeField]
        private int _raiseSalaryCost = 1;

        [SerializeField]
        private int _lowerSalaryCost = 1;

        [SerializeField]
        private int _raiseTaxCost = 2;

        [SerializeField]
        private int _lowerTaxCost = 2;

        [SerializeField]
        private int _printMoneyCost = 3;

        [SerializeField]
        private int _burnMoneyCost = 3;

        [SerializeField]
        private int _buildArenaCost = 5;

        [SerializeField]
        private int _useMineCost = 4;

        [SerializeField]
        private Button _raiseSalaryButton = null!;

        [SerializeField]
        private Button _lowerSalaryButton = null!;

        [SerializeField]
        private Button _raiseTaxButton = null!;

        [SerializeField]
        private Button _lowerTaxButton = null!;

        [SerializeField]
        private Button _printMoneyButton = null!;

        [SerializeField]
        private Button _burnMoneyButton = null!;

        [SerializeField]
        private Button _buildArenaButton = null!;

        [SerializeField]
        private Button _useMineButton = null!;

        [SerializeField]
        private string _toInventMultipleTrophyText = "Invent for [value] trophies";

        [SerializeField]
        private string _toInventSingleTrophyText = "Invent for 1 trophy";

        [SerializeField]
        private string _inventedText = "Invented";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _closeButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnClose);
            _raiseSalaryButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnRaiseSalary);
            _lowerSalaryButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnLowerSalary);
            _raiseTaxButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnRaiseTax);
            _lowerTaxButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnLowerTax);
            _printMoneyButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnPrintMoney);
            _burnMoneyButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnBurnMoney);
            _buildArenaButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnBuildArena);
            _useMineButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnUseMine);
        }

        protected override void OnShow(object? value)
        {
            _sub.Clear();
            Game.Instance.TechController.OnTechUnlocked.Subscribe(_ => UpdateButtons()).AddTo(_sub);
            Game.Instance.TreasureController.Treasures.Subscribe(OnTreasuresCountChanged).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _sub.Clear();
        }

        private void OnTreasuresCountChanged(int treasures)
        {
            _treasuresCount.text = treasures.ToString();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            _raiseSalaryButton.interactable = !Game.Instance.TechController.CanRaiseSalary.Value
                                              && Game.Instance.TreasureController.Treasures.Value >= _raiseSalaryCost;
            UpdateButtonText(_raiseSalaryButton, Game.Instance.TechController.CanRaiseSalary.Value, _raiseSalaryCost);
            _lowerSalaryButton.interactable = !Game.Instance.TechController.CanLowerSalary.Value
                                              && Game.Instance.TechController.CanRaiseSalary.Value
                                              && Game.Instance.TreasureController.Treasures.Value >= _lowerSalaryCost;
            UpdateButtonText(_lowerSalaryButton, Game.Instance.TechController.CanLowerSalary.Value, _lowerSalaryCost);
            _raiseTaxButton.interactable = !Game.Instance.TechController.CanRaiseTax.Value
                                           && Game.Instance.TechController.CanRaiseSalary.Value
                                           && Game.Instance.TreasureController.Treasures.Value >= _raiseTaxCost;
            UpdateButtonText(_raiseTaxButton, Game.Instance.TechController.CanRaiseTax.Value, _raiseTaxCost);
            _lowerTaxButton.interactable = !Game.Instance.TechController.CanLowerTax.Value
                                           && Game.Instance.TechController.CanRaiseTax.Value
                                           && Game.Instance.TreasureController.Treasures.Value >= _lowerTaxCost;
            UpdateButtonText(_lowerTaxButton, Game.Instance.TechController.CanLowerTax.Value, _lowerTaxCost);
            _printMoneyButton.interactable = !Game.Instance.TechController.CanPrintMoney.Value
                                             && Game.Instance.TechController.CanRaiseTax.Value
                                             && Game.Instance.TreasureController.Treasures.Value >= _printMoneyCost;
            UpdateButtonText(_printMoneyButton, Game.Instance.TechController.CanPrintMoney.Value, _printMoneyCost);
            _burnMoneyButton.interactable = !Game.Instance.TechController.CanBurnMoney.Value
                                            && Game.Instance.TechController.CanPrintMoney.Value
                                            && Game.Instance.TreasureController.Treasures.Value >= _burnMoneyCost;
            UpdateButtonText(_burnMoneyButton, Game.Instance.TechController.CanBurnMoney.Value, _burnMoneyCost);
            _useMineButton.interactable = !Game.Instance.TechController.CanUseMine.Value
                                          && Game.Instance.TechController.CanPrintMoney.Value
                                          && Game.Instance.TreasureController.Treasures.Value >= _useMineCost;

            UpdateButtonText(_useMineButton, Game.Instance.TechController.CanUseMine.Value, _useMineCost);
            _buildArenaButton.interactable = !Game.Instance.TechController.CanBuildArena.Value
                                             && Game.Instance.TechController.CanUseMine.Value
                                             && Game.Instance.TreasureController.Treasures.Value >= _buildArenaCost;
            UpdateButtonText(_buildArenaButton, Game.Instance.TechController.CanBuildArena.Value, _buildArenaCost);
        }

        private void UpdateButtonText(Button button, bool isInvented, int cost)
        {
            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = isInvented
                ? _inventedText
                : (cost == 1
                    ? _toInventSingleTrophyText
                    : _toInventMultipleTrophyText.Replace("[value]", cost.ToString()));
        }

        private void OnClose(Unit _)
        {
            Game.Instance.GameStateMachine.HideTechTree();
        }

        private void OnRaiseSalary(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_raiseSalaryCost);
            Game.Instance.TechController.UnlockRaiseSalary();
        }

        private void OnLowerSalary(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_lowerSalaryCost);
            Game.Instance.TechController.UnlockLowerSalary();
        }

        private void OnRaiseTax(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_raiseTaxCost);
            Game.Instance.TechController.UnlockRaiseTax();
        }

        private void OnLowerTax(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_lowerTaxCost);
            Game.Instance.TechController.UnlockLowerTax();
        }

        private void OnPrintMoney(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_printMoneyCost);
            Game.Instance.TechController.UnlockPrintMoney();
        }

        private void OnBurnMoney(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_burnMoneyCost);
            Game.Instance.TechController.UnlockBurnMoney();
        }

        private void OnBuildArena(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_buildArenaCost);
            Game.Instance.TechController.UnlockBuildArena();
        }

        private void OnUseMine(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_useMineCost);
            Game.Instance.TechController.UnlockMine();
        }
    }
}