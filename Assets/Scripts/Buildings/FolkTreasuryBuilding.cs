#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class FolkTreasuryBuilding : Building, IResourceStorage, ITaxController
    {
        private MoneyController _moneyController = null!;
        
        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private Sprite _moneySprite = null!;

        [SerializeField]
        private string _moneyTitle = "Money";

        public override BuildingType BuildingType => BuildingType.FolkTreasury;
        public Sprite SpriteIcon => _moneySprite;
        public string ResourceTitle => _moneyTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.MoneyController.FolkMoney;
        public IReadOnlyReactiveProperty<bool> CanRaiseTax => Game.Instance.TechController.CanRaiseTax;
        public IReadOnlyReactiveProperty<bool> CanLowerTax => Game.Instance.TechController.CanLowerTax;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _moneyController = Game.Instance.MoneyController;
            _moneyController.FolkMoney
                .TakeUntilDisable(this)
                .Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }
        
        public void RaiseTax() => Game.Instance.FolkManager.RaiseTax();
        public void LowerTax() => Game.Instance.FolkManager.LowerTax();
    }
}