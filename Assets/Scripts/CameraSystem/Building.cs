#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    public class Building : InitializableBehaviour
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