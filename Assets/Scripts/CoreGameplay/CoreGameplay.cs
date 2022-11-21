#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [RequireComponent(typeof(FolkManager))]
    [RequireComponent(typeof(ArmyManager))]
    public sealed class CoreGameplay : InitializableBehaviour
    {
        [SerializeField]
        private int _foodPerPerson = 2;

        [SerializeField]
        private int _newFolkForFoodCost = 10;

        [SerializeField]
        private int _newArmyForFoodCost = 10;

        public int FoodPerPerson => _foodPerPerson;
        public int NewFolkForFoodCost => _newFolkForFoodCost;
        public int NewArmyForFoodCost => _newArmyForFoodCost;

        public FolkManager FolkManager { get; private set; } = null!;
        public ArmyManager ArmyManager { get; private set; } = null!;

        private void Awake()
        {
            FolkManager = GetComponent<FolkManager>();
            ArmyManager = GetComponent<ArmyManager>();
        }

        public void InitializeGameData()
        {
            FolkManager.InitializeGameData();
            ArmyManager.InitializeGameData();
        }

        public void StartRound()
        {
            FolkManager.StartRound();
            ArmyManager.StartRound();
        }

        public void EndRound()
        {
            FolkManager.EndRound();
            ArmyManager.EndRound();
        }
    }
}