#nullable enable
using NovemberProject.Time;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public interface IConstructableBuilding
    {
        public IReadOnlyReactiveProperty<ConstructableState> ConstructableState { get; }
        public int ConstructionCost { get; }
        public Sprite ResourceImage { get; }
        public IReadOnlyTimer? ConstructionTimer { get; }
        public void StartConstruction();
    }
}