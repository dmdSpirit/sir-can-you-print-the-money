#nullable enable
using UniRx;

namespace NovemberProject.Buildings.UI
{
    public interface ISalaryController
    {
        public IReadOnlyReactiveProperty<bool> CanRaiseSalary { get; }
        public IReadOnlyReactiveProperty<bool> CanLowerSalary { get; }
        public void RaiseSalary();
        public void LowerSalary();
    }
}