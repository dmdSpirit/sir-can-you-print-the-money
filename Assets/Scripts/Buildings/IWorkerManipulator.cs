#nullable enable
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IWorkerManipulator
    {
        public void AddWorker();
        public void RemoveWorker();
        public IReadOnlyReactiveProperty<int> WorkerCount { get; }
        public int MaxWorkerCount { get; }
        public bool HasMaxWorkerCount { get; }
        public string WorkersTitle { get; }
        public bool CanAddWorker();
        public bool CanRemoveWorker();
    }
}