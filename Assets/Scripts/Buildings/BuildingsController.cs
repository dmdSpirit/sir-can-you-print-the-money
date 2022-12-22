#nullable enable
using System;
using System.Collections.Generic;

namespace NovemberProject.Buildings
{
    public sealed class BuildingsController
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

        public T GetBuilding<T>() where T : Building
        {
            foreach (Building building in _buildings.Values)
            {
                if (building is T typedBuilding)
                {
                    return typedBuilding;
                }
            }

            throw new Exception($"No building of type {typeof(T)} registered");
        }
    }
}