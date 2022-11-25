#nullable enable
using NovemberProject.TechTree;
using UniRx;

namespace NovemberProject.Buildings
{
    public sealed class ChangeSalaryAbility : IUnlockableAbility
    {
        public IReadOnlyReactiveProperty<bool> IsUnlocked { get; }

        public void Unlock()
        {
        }
    }
}