#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System
{
    [RequireComponent(typeof(FolkManager))]
    [RequireComponent(typeof(ArmyManager))]
    public sealed class CoreGameplay : InitializableBehaviour
    {
        private FolkManager _folkManager = null!;
        private ArmyManager _armyManager = null!;

        [SerializeField]
        private int _foodPerPerson = 2;

        [SerializeField]
        private int _newFolkForFoodCost = 10;
        [SerializeField]
        private int _newArmyForFoodCost = 10;

        public int FoodPerPerson => _foodPerPerson;
        public int NewFolkForFoodCost => _newFolkForFoodCost;
        public int NewArmyForFoodCost => _newFolkForFoodCost;

        private void Awake()
        {
            _folkManager = GetComponent<FolkManager>();
            _armyManager = GetComponent<ArmyManager>();
        }

        public void StartRound()
        {
            _folkManager.StartRound();
            _armyManager.StartRound();
        }

        public void EndRound()
        {
            _folkManager.EndRound();
            _armyManager.EndRound();
        }
    }
}