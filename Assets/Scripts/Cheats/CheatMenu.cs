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
            if (_generatedButtons.Count != 0)
            {
                return;
            }

            Debug.LogWarning("No cheat buttons to show");
            Hide();
        }

        protected override void OnHide()
        {
        }

        private void PrepareButtonsList()
        {
            _cheatButtons.Add(new CheatButtonInfo("Print 10 Money", Print10Money));
            _cheatButtons.Add(new CheatButtonInfo("Add 10 stone", Add10Stone));
        }

        private static void Print10Money()
        {
            Game.Instance.MoneyController.AddGovernmentMoney(10);
        }

        private static void Add10Stone()
        {
            Game.Instance.StoneController.AddStone(10);
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
    }
}