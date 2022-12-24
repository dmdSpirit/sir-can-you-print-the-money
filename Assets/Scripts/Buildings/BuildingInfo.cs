#nullable enable
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class BuildingInfo : MonoBehaviour, IBuildingInfo
    {
        [SerializeField]
        private string _title = null!;

        [SerializeField]
        private string _description = null!;

        [SerializeField]
        private Sprite _image = null!;

        public string Title => _title;
        public string Description => _description;
        public Sprite Image => _image;
    }
}