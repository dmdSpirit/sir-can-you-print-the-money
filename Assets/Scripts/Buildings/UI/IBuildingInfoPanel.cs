#nullable enable
using NovemberProject.CommonUIStuff;

namespace NovemberProject.Buildings.UI
{
    public interface IBuildingInfoPanel : IUIScreen
    {
        public void SetBuilding(Building building);
    }
}