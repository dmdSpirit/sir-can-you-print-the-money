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
            _cheatButtons.Add(new CheatButtonInfo("Print 10 Money", Print10Money));
            _cheatButtons.Add(new CheatButtonInfo("Add farmer", AddFarmer));
            _cheatButtons.Add(new CheatButtonInfo("Remove farmer", RemoveFarmer));
            _cheatButtons.Add(new CheatButtonInfo("Add trader", AddMarketWorker));
            _cheatButtons.Add(new CheatButtonInfo("Remove trader", RemoveMarketWorker));
            _cheatButtons.Add(new CheatButtonInfo("Buy folk", BuyFolk));
            _cheatButtons.Add(new CheatButtonInfo("Buy army", BuyArmy));
            _cheatButtons.Add(new CheatButtonInfo("Raise taxes", RaiseTax));
            _cheatButtons.Add(new CheatButtonInfo("Lower taxes", LowerTax));
            _cheatButtons.Add(new CheatButtonInfo("Raise salary", RaiseSalary));
            _cheatButtons.Add(new CheatButtonInfo("Lower salary", LowerSalary));
        }

        private void Print10Money()
        {
            Game.Instance.MoneyController.AddGovernmentMoney(10);
        }

        private void AddFarmer()
        {
            if (Game.Instance.CoreGameplay.FolkManager.IdleFolk.Value == 0)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.AddFolkToFarm();
        }

        private void RemoveFarmer()
        {
            if (Game.Instance.CoreGameplay.FolkManager.FarmFolk.Value == 0)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.RemoveFolkFromFarm();
        }

        private void AddMarketWorker()
        {
            if (Game.Instance.CoreGameplay.FolkManager.MarketFolk.Value == 1)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.AddFolkToMarket();
        }

        private void RemoveMarketWorker()
        {
            if (Game.Instance.CoreGameplay.FolkManager.MarketFolk.Value == 0)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.RemoveFolkFromMarket();
        }

        private void RaiseTax()
        {
            Game.Instance.CoreGameplay.FolkManager.RaiseTax();
        }

        private void LowerTax()
        {
            if (Game.Instance.CoreGameplay.FolkManager.Tax.Value <= 1)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.LowerTax();
        }

        private void RaiseSalary()
        {
            Game.Instance.CoreGameplay.ArmyManager.RaiseSalary();
        }

        private void LowerSalary()
        {
            if (Game.Instance.CoreGameplay.ArmyManager.Salary.Value <= 1)
            {
                return;
            }

            Game.Instance.CoreGameplay.ArmyManager.LowerSalary();
        }

        private void BuyFolk()
        {
            if (Game.Instance.FoodController.FolkFood.Value < Game.Instance.CoreGameplay.NewFolkForFoodCost)
            {
                return;
            }

            Game.Instance.CoreGameplay.FolkManager.BuyFolkForFood();
        }

        private void BuyArmy()
        {
            if (Game.Instance.FoodController.ArmyFood.Value < Game.Instance.CoreGameplay.NewArmyForFoodCost)
            {
                return;
            }

            Game.Instance.CoreGameplay.ArmyManager.BuyArmyForFood();
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