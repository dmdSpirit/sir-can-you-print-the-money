#nullable enable
using System.Collections.Generic;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;

namespace NovemberProject.Money
{
    public sealed class BuildingsController : InitializableBehaviour
    {
        private readonly Dictionary<BuildingType, Building> _buildings = new();

        public void RegisterBuilding(Building building)
        {
            if (building.BuildingType == BuildingType.None)
            {
                return;
            }

            _buildings.Add(building.BuildingType, building);
        }

        public Building GetBuilding(BuildingType buildingType)
        {
            return _buildings[buildingType];
        }
    }
}