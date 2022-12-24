#nullable enable
using System;
using NovemberProject.Buildings.UI;
using NovemberProject.Core;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class GuardTowerBuilding : Building, IResourceStorage, IIncomingAttack
    {
        private ArmyManager _armyManager=null!;
        private CombatController _combatController = null!;

        [SerializeField]
        private TMP_Text _armyText = null!;

        [SerializeField]
        private Sprite _guardImage = null!;

        [SerializeField]
        private string _guardTitle = "Guards";

        // public override BuildingType BuildingType => BuildingType.GuardTower;

        public Sprite SpriteIcon => _guardImage;
        public IReadOnlyReactiveProperty<int> ResourceCount => _armyManager.GuardsCount;
        public string ResourceTitle => _guardTitle;
        public IReadOnlyReactiveProperty<int> Defenders => _armyManager.GuardsCount;
        public IReadOnlyTimer? AttackTimer => _combatController.AttackTimer;
        public int Attackers => _combatController.NextAttackersCount;

        public float WinProbability =>
            1 - _combatController.GetAttackersWinProbability(Attackers, Defenders.Value);

        public IObservable<Unit> OnNewAttack => _combatController.OnNewAttack;

        [Inject]
        private void Construct(ArmyManager armyManager, CombatController combatController)
        {
            _armyManager = armyManager;
            _combatController = combatController;
            _armyManager.GuardsCount.Subscribe(OnArmyCountChanged);
        }

        private void OnArmyCountChanged(int army)
        {
            _armyText.text = army.ToString();
        }
    }
}