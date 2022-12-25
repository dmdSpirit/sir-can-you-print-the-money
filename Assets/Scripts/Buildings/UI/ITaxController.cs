#nullable enable
using UniRx;

namespace NovemberProject.Buildings.UI
{
    public interface ITaxController : IBuildingFunction
    {
        public IReadOnlyReactiveProperty<bool> CanRaiseTax { get; }
        public IReadOnlyReactiveProperty<bool> CanLowerTax { get; }
        public void RaiseTax();
        public void LowerTax();
    }
}