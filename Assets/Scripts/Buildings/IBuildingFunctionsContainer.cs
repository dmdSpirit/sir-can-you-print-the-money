#nullable enable
using System.Collections.Generic;

namespace NovemberProject.Buildings
{
    public interface IBuildingFunctionsContainer
    {
        public IReadOnlyList<IBuildingFunction> BuildingFunctions { get; }
    }
}