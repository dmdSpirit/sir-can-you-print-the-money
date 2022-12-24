#nullable enable
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class ResourceStorage : MonoBehaviour, IResourceStorage
    {
        private ResourceProvider _resourceProvider = null!;

        [SerializeField]
        private ResourceType _resourceType;

        [SerializeField]
        private TMP_Text _countText = null!;

        public Sprite SpriteIcon => _resourceProvider.GetResourceIcon(_resourceType);
        public IReadOnlyReactiveProperty<int> ResourceCount => _resourceProvider.GetResourceQuantity(_resourceType);
        public string ResourceTitle => _resourceProvider.GetResourceTitle(_resourceType);

        [Inject]
        private void Construct(ResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
            ResourceCount.Subscribe(OnResourceCountChanged);
        }

        private void OnResourceCountChanged(int resourceCount)
        {
            _countText.text = resourceCount.ToString();
        }
    }
}