#nullable enable
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public interface IResourceStorage : IBuildingFunction
    {
        public Sprite SpriteIcon { get; }
        public IReadOnlyReactiveProperty<int> ResourceCount { get; }
        public string ResourceTitle { get; }
    }
}