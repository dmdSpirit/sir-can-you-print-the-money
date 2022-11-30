#nullable enable
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IMineWorkerManipulator
    {
        public IReadOnlyReactiveProperty<bool> CanUseMine { get; }
        public void AddWorker();
        public void RemoveWorker();
        public IReadOnlyReactiveProperty<int> WorkerCount { get; }
        public string WorkersTitle { get; }
        public bool CanAddWorker();
        public bool CanRemoveWorker();
        
    }
}