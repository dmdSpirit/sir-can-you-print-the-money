#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using NovemberProject.TechTree;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class TechTreeScreen : UIElement<object?>
    {
        private readonly CompositeDisposable _sub = new();

        private GameStateMachine _gameStateMachine = null!;
        private TechController _techController = null!;

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

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, TechController techController)
        {
            _gameStateMachine = gameStateMachine;
            _techController = techController;
        }

        private void Start()
        {
            _closeButton.OnClickAsObservable()
                .Subscribe(OnClose);
            _raiseSalaryButton.OnClickAsObservable()
                .Subscribe(OnRaiseSalary);
            _lowerSalaryButton.OnClickAsObservable()
                .Subscribe(OnLowerSalary);
            _raiseTaxButton.OnClickAsObservable()
                .Subscribe(OnRaiseTax);
            _lowerTaxButton.OnClickAsObservable()
                .Subscribe(OnLowerTax);
            _printMoneyButton.OnClickAsObservable()
                .Subscribe(OnPrintMoney);
            _burnMoneyButton.OnClickAsObservable()
                .Subscribe(OnBurnMoney);
            _buildArenaButton.OnClickAsObservable()
                .Subscribe(OnBuildArena);
            _useMineButton.OnClickAsObservable()
                .Subscribe(OnUseMine);
        }

        protected override void OnShow(object? value)
        {
            _sub.Clear();
            _techController.OnTechUnlocked.Subscribe(_ => UpdateButtons()).AddTo(_sub);
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
            _raiseSalaryButton.interactable = !_techController.CanRaiseSalary.Value
                                              && Game.Instance.TreasureController.Treasures.Value >= _raiseSalaryCost;
            UpdateButtonText(_raiseSalaryButton, _techController.CanRaiseSalary.Value, _raiseSalaryCost);
            _lowerSalaryButton.interactable = !_techController.CanLowerSalary.Value
                                              && _techController.CanRaiseSalary.Value
                                              && Game.Instance.TreasureController.Treasures.Value >= _lowerSalaryCost;
            UpdateButtonText(_lowerSalaryButton, _techController.CanLowerSalary.Value, _lowerSalaryCost);
            _raiseTaxButton.interactable = !_techController.CanRaiseTax.Value
                                           && _techController.CanRaiseSalary.Value
                                           && Game.Instance.TreasureController.Treasures.Value >= _raiseTaxCost;
            UpdateButtonText(_raiseTaxButton, _techController.CanRaiseTax.Value, _raiseTaxCost);
            _lowerTaxButton.interactable = !_techController.CanLowerTax.Value
                                           && _techController.CanRaiseTax.Value
                                           && Game.Instance.TreasureController.Treasures.Value >= _lowerTaxCost;
            UpdateButtonText(_lowerTaxButton, _techController.CanLowerTax.Value, _lowerTaxCost);
            _printMoneyButton.interactable = !_techController.CanPrintMoney.Value
                                             && _techController.CanRaiseTax.Value
                                             && Game.Instance.TreasureController.Treasures.Value >= _printMoneyCost;
            UpdateButtonText(_printMoneyButton, _techController.CanPrintMoney.Value, _printMoneyCost);
            _burnMoneyButton.interactable = !_techController.CanBurnMoney.Value
                                            && _techController.CanPrintMoney.Value
                                            && Game.Instance.TreasureController.Treasures.Value >= _burnMoneyCost;
            UpdateButtonText(_burnMoneyButton, _techController.CanBurnMoney.Value, _burnMoneyCost);
            _useMineButton.interactable = !_techController.CanUseMine.Value
                                          && _techController.CanPrintMoney.Value
                                          && Game.Instance.TreasureController.Treasures.Value >= _useMineCost;

            UpdateButtonText(_useMineButton, _techController.CanUseMine.Value, _useMineCost);
            _buildArenaButton.interactable = !_techController.CanBuildArena.Value
                                             && _techController.CanUseMine.Value
                                             && Game.Instance.TreasureController.Treasures.Value >= _buildArenaCost;
            UpdateButtonText(_buildArenaButton, _techController.CanBuildArena.Value, _buildArenaCost);
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
            _gameStateMachine.HideTechTree();
        }

        private void OnRaiseSalary(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_raiseSalaryCost);
            _techController.UnlockRaiseSalary();
        }

        private void OnLowerSalary(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_lowerSalaryCost);
            _techController.UnlockLowerSalary();
        }

        private void OnRaiseTax(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_raiseTaxCost);
            _techController.UnlockRaiseTax();
        }

        private void OnLowerTax(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_lowerTaxCost);
            _techController.UnlockLowerTax();
        }

        private void OnPrintMoney(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_printMoneyCost);
            _techController.UnlockPrintMoney();
        }

        private void OnBurnMoney(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_burnMoneyCost);
            _techController.UnlockBurnMoney();
        }

        private void OnBuildArena(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_buildArenaCost);
            _techController.UnlockBuildArena();
        }

        private void OnUseMine(Unit _)
        {
            Game.Instance.TreasureController.SpendTreasures(_useMineCost);
            _techController.UnlockMine();
        }
    }
}