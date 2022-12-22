#nullable enable
using UniRx;

namespace NovemberProject.Buildings
{
    public interface ISelectable
    {
        public IReadOnlyReactiveProperty<bool> IsSelected { get; }
        public void Select();
        public void Unselect();
    }
}