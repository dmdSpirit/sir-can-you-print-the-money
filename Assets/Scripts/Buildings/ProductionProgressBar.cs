#nullable enable
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.Buildings
{
    public sealed class ProductionProgressBar : MonoBehaviour
    {
        private IDisposable? _productionSub;
        private bool _isProgressShown;
        private IProducer _producer = null!;

        [SerializeField]
        private Image _progressImage = null!;

        [SerializeField]
        private TMP_Text _producedValue = null!;

        [SerializeField]
        private GameObject _progressBar = null!;

        [SerializeField]
        private Building _building = null!;

        private void Start()
        {
            _producer = (IProducer)_building;
            _progressBar.SetActive(false);
            _producer.IsProducing
                .Subscribe(OnIsProducingChanged);
            _producedValue.gameObject.SetActive(_producer.ShowProducedValue);
        }

        private void Update()
        {
            if (!_isProgressShown)
            {
                return;
            }

            Assert.IsNotNull(_producer.ProductionTimer);
            _progressImage.fillAmount = _producer.ProductionTimer!.ProgressRate;
        }

        private void OnIsProducingChanged(bool isProducing)
        {
            if (_isProgressShown == isProducing)
            {
                return;
            }

            _productionSub?.Dispose();
            _progressBar.SetActive(isProducing);
            if (isProducing && _producer.ShowProducedValue)
            {
                _productionSub = _producer.ProducedValue.Subscribe(OnProducedValueChanged);
            }

            _isProgressShown = isProducing;
        }

        private void OnProducedValueChanged(int producedValue)
        {
            _producedValue.text = producedValue.ToString();
        }
    }
}