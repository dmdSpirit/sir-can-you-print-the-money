#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;

namespace NovemberProject.CoreGameplay
{
    public sealed class Expeditions : InitializableBehaviour
    {
        private readonly ReactiveProperty<bool> _isExpeditionActive = new();

        public IReadOnlyReactiveProperty<bool> IsExpeditionActive => _isExpeditionActive;
    }
}