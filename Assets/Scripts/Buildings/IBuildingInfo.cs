#nullable enable
using UnityEngine;

namespace NovemberProject.Buildings
{
    public interface IBuildingInfo
    {
        public string Title { get; }
        public string Description { get; }
        public Sprite Image { get; }  
    }
}