#nullable enable
using System;
using NovemberProject.Core;
using NovemberProject.Core.FolkManagement;

namespace NovemberProject.Buildings
{
    public sealed class UnitManagerProvider
    {
        private readonly ArmyManager _armyManager;
        private readonly FolkManager _folkManager;

        public UnitManagerProvider(ArmyManager armyManager, FolkManager folkManager)
        {
            _armyManager = armyManager;
            _folkManager = folkManager;
        }

        public IUnitManager GetUnitManagerFor(UnitType unitType)
        {
            return unitType switch
            {
                UnitType.Folk => _folkManager,
                UnitType.Army => _armyManager,
                _ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null)
            };
        }
    }
}