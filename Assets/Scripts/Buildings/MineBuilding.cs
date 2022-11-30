#nullable enable
using NovemberProject.System;
using UniRx;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.Buildings
{
    public sealed class MineBuilding : Building, IMineWorkerManipulator
    {
        public override BuildingType BuildingType => BuildingType.Mine;

        public IReadOnlyReactiveProperty<bool> CanUseMine => Game.Instance.TechController.CanUseMine;
        public void AddWorker()
        {
            Game.Instance.FolkManager.AddFolkToMine();
        }

        public void RemoveWorker()
        {
            Game.Instance.FolkManager.RemoveFolkFromMine();
        }

        public IReadOnlyReactiveProperty<int> WorkerCount { get; }
        public string WorkersTitle { get; }
        public bool CanAddWorker()
        {
            throw new NotImplementedException();
        }

        public bool CanRemoveWorker()
        {
            throw new NotImplementedException();
        }
    }
}