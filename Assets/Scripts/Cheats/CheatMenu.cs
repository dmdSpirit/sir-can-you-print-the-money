#nullable enable
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Cheats
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public sealed class CheatMenu : UIElement<object?>
    {
        private readonly List<CheatButtonInfo> _cheatButtons = new();
        private readonly List<CheatButton> _generatedButtons = new();

        [SerializeField]
        private CheatButton _buttonPrefab = null!;

        [SerializeField]
        private Transform _buttonsParent = null!;

        [SerializeField]
        private int _moneyToMove = 10;

        private bool _isSizeFitterRefreshNeeded;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClearOldButtons();
            PrepareButtonsList();
            GenerateButtons();
        }

        protected override void OnShow(object? _)
        {
            if (_generatedButtons.Count == 0)
            {
                Debug.LogWarning("No cheat buttons to show");
                Hide();
            }

            // Bug (Stas): parent layout group is not updated properly first time.
        }

        protected override void OnHide()
        {
        }

        private void PrepareButtonsList()
        {
            _cheatButtons.Add(new CheatButtonInfo("Gov to Army", MoveMoneyGovToArmy));
            _cheatButtons.Add(new CheatButtonInfo("Army to Folk", MoveMoneyArmyToFolk));
            _cheatButtons.Add(new CheatButtonInfo("Folk to Gov", MoveMoneyFolkToGov));
        }

        private void GenerateButtons()
        {
            foreach (CheatButtonInfo cheatButtonInfo in _cheatButtons)
            {
                CheatButton button = Instantiate(_buttonPrefab, _buttonsParent);
                button.Show(cheatButtonInfo);
                _generatedButtons.Add(button);
            }
        }

        private void ClearOldButtons()
        {
            _cheatButtons.Clear();
            _generatedButtons.Clear();

            var oldButtons = _buttonsParent.GetComponentsInChildren<CheatButton>();
            if (!HasOldButtons())
            {
                return;
            }

            foreach (CheatButton? button in oldButtons)
            {
                if (button == null)
                {
                    continue;
                }

                Destroy(button.gameObject);
            }

            bool HasOldButtons() => oldButtons is { Length: > 0 };
        }

        private void MoveMoneyGovToArmy()
        {
            if (Game.Instance.MoneyController.GovernmentMoney.Value < _moneyToMove)
            {
                return;
            }

            Game.Instance.MoneyController.TransferMoneyFromGovernmentToArmy(_moneyToMove);
        }

        private void MoveMoneyArmyToFolk()
        {
            if (Game.Instance.MoneyController.ArmyMoney.Value < _moneyToMove)
            {
                return;
            }

            Game.Instance.MoneyController.TransferMoneyFromArmyToFolk(_moneyToMove);
        }

        private void MoveMoneyFolkToGov()
        {
            if (Game.Instance.MoneyController.FolkMoney.Value < _moneyToMove)
            {
                return;
            }

            Game.Instance.MoneyController.TransferMoneyFromFolkToGovernment(_moneyToMove);
        }
    }
}