#nullable enable
using NovemberProject.Buildings.UI;
using NovemberProject.Core;
using NovemberProject.Core.FolkManagement;
using NovemberProject.System;
using NovemberProject.TechTree;
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
        private TechController _techController = null!;

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
        public IReadOnlyReactiveProperty<bool> CanRaiseTax => _techController.CanRaiseTax;
        public IReadOnlyReactiveProperty<bool> CanLowerTax => _techController.CanLowerTax;

        [Inject]
        private void Construct(FolkManager folkManager, MoneyController moneyController, TechController techController)
        {
            _folkManager = folkManager;
            _moneyController = moneyController;
            _techController = techController;
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