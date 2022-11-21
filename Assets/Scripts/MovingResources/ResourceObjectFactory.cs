#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.MovingResources
{
    public sealed class ResourceObjectFactory : InitializableBehaviour
    {
        [SerializeField]
        private GameObject _coinPrefab = null!;

        [SerializeField]
        private GameObject _foodPrefab = null!;

        public GameObject Coin() => Instantiate(_coinPrefab, transform);
        public GameObject Food() => Instantiate(_foodPrefab, transform);
    }
}