#nullable enable
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using NovemberProject.Core;
using NovemberProject.GameStates;
using NovemberProject.System;
using NovemberProject.Treasures;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Cheats
{
    public interface ICheatMenu : IUIScreen{}
    
    [RequireComponent(typeof(ContentSizeFitter))]
    public sealed class CheatMenu : UIScreen, ICheatMenu
    {
        private readonly List<CheatButtonInfo> _cheatButtons = new();
        private readonly List<CheatButton> _generatedButtons = new();

        private MoneyController _moneyController = null!;
        private GameStateMachine _gameStateMachine = null!;
        private StoneController _stoneController = null!;
        private TreasureController _treasureController = null!;

        [SerializeField]
        private CheatButton _buttonPrefab = null!;

        [SerializeField]
        private Transform _buttonsParent = null!;

        private bool _isSizeFitterRefreshNeeded;

        [Inject]
        private void Construct(MoneyController moneyController, GameStateMachine gameStateMachine,
            StoneController stoneController, TreasureController treasureController)
        {
            _moneyController = moneyController;
            _gameStateMachine = gameStateMachine;
            _stoneController = stoneController;
            _treasureController = treasureController;
        }

        private void Start()
        {
            ClearOldButtons();
            PrepareButtonsList();
            GenerateButtons();
        }

        protected override void OnShow()
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
            _cheatButtons.Add(new CheatButtonInfo("Win", Win));
            _cheatButtons.Add(new CheatButtonInfo("Add treasure", AddTreasure));
        }

        private void Print10Money()
        {
            _moneyController.PrintMoney(10);
        }

        private void Add10Stone()
        {
            _stoneController.AddStone(10);
        }

        private void AddTreasure()
        {
            _treasureController.AddTreasures(1);
        }

        private void Win()
        {
            _gameStateMachine.Victory();
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