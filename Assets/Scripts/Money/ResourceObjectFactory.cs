#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.Money
{
    public sealed class ResourceObjectFactory : InitializableBehaviour
    {
        [SerializeField]
        private GameObject _coinPrefab = null!;
        [SerializeField]
        private GameObject _foodPrefab = null!;

        public GameObject Coin()
        {
            return Instantiate(_coinPrefab, transform);
        }

        public GameObject Food()
        {
            return Instantiate(_foodPrefab, transform);
        }
    }
}