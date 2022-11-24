#nullable enable
using UniRx;

namespace NovemberProject.TechTree
{
    public interface IUnlockableAbility
    {
        public IReadOnlyReactiveProperty<bool> IsUnlocked { get; }
        public void Unlock();
    }
}