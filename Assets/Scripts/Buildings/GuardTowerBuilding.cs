#nullable enable
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class GuardTowerBuilding : Building
    {
        [SerializeField]
        private TMP_Text _armyText = null!;

        public override BuildingType BuildingType => BuildingType.GuardTower;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.ArmyManager.ArmyCount
                .TakeUntilDisable(this)
                .Subscribe(OnArmyCountChanged);
        }

        private void OnArmyCountChanged(int army)
        {
            _armyText.text = army.ToString();
        }
    }
}