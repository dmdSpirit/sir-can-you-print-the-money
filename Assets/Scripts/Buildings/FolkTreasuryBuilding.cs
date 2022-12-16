#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class FolkTreasuryBuilding : Building, IResourceStorage, ITaxController
    {
        private FolkManager _folkManager = null!;
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
        public IReadOnlyReactiveProperty<int> ResourceCount => _moneyController.FolkMoney;
        public IReadOnlyReactiveProperty<bool> CanRaiseTax => Game.Instance.TechController.CanRaiseTax;
        public IReadOnlyReactiveProperty<bool> CanLowerTax => Game.Instance.TechController.CanLowerTax;

        [Inject]
        private void Construct(FolkManager folkManager, MoneyController moneyController)
        {
            _folkManager = folkManager;
            _moneyController = moneyController;
            _moneyController.FolkMoney.Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }

        public void RaiseTax() => _folkManager.RaiseTax();
        public void LowerTax() => _folkManager.LowerTax();
    }
}