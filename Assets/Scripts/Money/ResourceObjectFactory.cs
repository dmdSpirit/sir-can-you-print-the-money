#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Money
{
    public sealed class ResourceObjectFactory : InitializableBehaviour
    {
        [SerializeField]
        private Sprite _coinImage = null!;

        [SerializeField]
        private GameObject _resourcePrefab = null!;

        public GameObject Coin()
        {
            GameObject coin = Instantiate(_resourcePrefab, transform);
            var image = coin.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = _coinImage;
            }

            return coin;
        }
    }
}