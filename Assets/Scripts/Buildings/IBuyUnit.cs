#nullable enable
namespace NovemberProject.Buildings
{
    public interface IBuyUnit : IBuildingFunction
    {
        public void BuyUnit();
        public bool CanBuyUnit { get; }
        public string BuyUnitTitle { get; }
        public string BuyUnitButtonText { get; }
    }
}