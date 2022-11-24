#nullable enable
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IProducer
    {
        public IReadOnlyReactiveProperty<int> ProducedValue { get; }
        public IReadOnlyReactiveProperty<bool> IsProducing { get; }
        public Timer? ProductionTimer { get; }
    }
}