#nullable enable
using System;
using NovemberProject.Buildings.UI;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class GuardTowerBuilding : Building, IResourceStorage, IIncomingAttack
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
        public IReadOnlyReactiveProperty<int> Defenders => Game.Instance.ArmyManager.GuardsCount;
        public IReadOnlyTimer? AttackTimer => Game.Instance.CombatController.AttackTimer;
        public int Attackers => Game.Instance.CombatController.NextAttackersCount();

        public float WinProbability =>
            1 - Game.Instance.CombatController.GetAttackersWinProbability(Attackers, Defenders.Value);

        public IObservable<Unit> OnNewAttack => Game.Instance.CombatController.OnNewAttack;

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