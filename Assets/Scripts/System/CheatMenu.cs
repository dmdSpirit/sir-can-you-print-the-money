#nullable enable
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System
{
    public sealed class CheatMenu : InitializableBehaviour
    {
        private readonly List<CheatButtonInfo> _cheatButtons = new();
        private readonly List<CheatButton> _generatedButtons = new();

        [SerializeField]
        private CheatButton _buttonPrefab = null!;

        [SerializeField]
        private Transform _buttonsParent = null!;

        protected override void Initialize()
        {
            base.Initialize();

            ClearOldButtons();
            PrepareButtonsList();
            GenerateButtons();
            if (_generatedButtons.Count == 0)
            {
                
            }
        }

        private void PrepareButtonsList()
        {
            _cheatButtons.Add(new CheatButtonInfo("Test", () => Debug.Log("Test")));
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
            if (oldButtons == null || oldButtons.Length == 0)
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
        }
    }
}