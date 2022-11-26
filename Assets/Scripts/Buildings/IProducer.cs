#nullable enable
using NovemberProject.Time;
using UniRx;

namespace NovemberProject.Buildings
{
    public interface IProducer
    {
        public bool ShowProducedValue { get; }
        public IReadOnlyReactiveProperty<int>? ProducedValue { get; }
        public IReadOnlyReactiveProperty<bool> IsProducing { get; }
        public IReadOnlyTimer? ProductionTimer { get; }
    }
}