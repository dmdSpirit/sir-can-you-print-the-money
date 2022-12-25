#nullable enable
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IExpeditionSender : IWorkerManipulator, IBuildingFunction
    {
        public IReadOnlyReactiveProperty<bool> IsActive { get; }
        public IReadOnlyReactiveProperty<bool> IsExpeditionActive { get; }
        public IReadOnlyReactiveProperty<bool> CanBeSentToExpedition { get; }
        public IReadOnlyTimer? ExpeditionTimer { get; }
        public float WinProbability { get; }
        public int Defenders { get; }
        public int Reward { get; }
    }
}