#nullable enable
using UniRx;

namespace NovemberProject.Buildings.UI
{
    public interface IMoneyPrinter
    {
        public IReadOnlyReactiveProperty<bool> CanPrintMoney { get; }
        public IReadOnlyReactiveProperty<bool> CanBurnMoney { get; }
        public void PrintMoney();
        public void BurnMoney();
    }
}