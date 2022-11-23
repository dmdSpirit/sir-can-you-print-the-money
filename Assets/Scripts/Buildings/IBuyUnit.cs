#nullable enable
namespace NovemberProject.Buildings
{
    public interface IBuyUnit
    {
        public void BuyUnit();
        public bool CanBuyUnit { get; }
        public string BuyUnitTitle { get; }
        public string BuyUnitButtonText { get; }
    }
}