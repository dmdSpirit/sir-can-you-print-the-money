#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class GuardTowerBuilding : Building, IResourceStorage
    {
        [SerializeField]
        private TMP_Text _armyText = null!;

        [SerializeField]
        private Sprite _guardImage = null!;

        [SerializeField]
        private string _guardTitle = "Guards";

        public override BuildingType BuildingType => BuildingType.GuardTower;

        public Sprite SpriteIcon => _guardImage;
        public IReadOnlyReactiveProperty<int> ResourceCount => Game.Instance.ArmyManager.GuardsCount;
        public string ResourceTitle => _guardTitle;
        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.ArmyManager.GuardsCount
                .TakeUntilDisable(this)
                .Subscribe(OnArmyCountChanged);
        }

        private void OnArmyCountChanged(int army)
        {
            _armyText.text = army.ToString();
        }

    }
}