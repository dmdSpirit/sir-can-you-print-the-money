#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.Buildings
{
    public sealed class GovernmentTreasuryBuilding : Building, IResourceStorage
    {
        private MoneyController _moneyController = null!;

        [SerializeField]
        private TMP_Text _moneyText = null!;

        [SerializeField]
        private Sprite _moneySprite = null!;

        [SerializeField]
        private string _moneyTitle = "Money";

        public override BuildingType BuildingType => BuildingType.GovernmentTreasury;
        public Sprite SpriteIcon => _moneySprite;
        public string ResourceTitle => _moneyTitle;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.MoneyController.GovernmentMoney;
        public ChangeSalaryAbility ChangeSalaryAbility { get; private set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _moneyController = Game.Instance.MoneyController;
            _moneyController.GovernmentMoney
                .TakeUntilDisable(this)
                .Subscribe(OnMoneyChanged);
        }

        private void OnMoneyChanged(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}