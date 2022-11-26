#nullable enable
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IExpeditionSender : IWorkerManipulator
    {
        public IReadOnlyReactiveProperty<bool> IsExpeditionActive { get; }
        public IReadOnlyReactiveProperty<bool> CanBeSentToExpedition { get; }
        public IReadOnlyTimer? ExpeditionTimer { get; }
    }
}