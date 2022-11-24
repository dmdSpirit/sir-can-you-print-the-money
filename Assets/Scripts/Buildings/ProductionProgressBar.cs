#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.Buildings
{
    public sealed class ProductionProgressBar : InitializableBehaviour
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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _producer = (IProducer)_building;
            _progressBar.SetActive(false);
            _producer.IsProducing
                .TakeUntilDisable(this)
                .Subscribe(OnIsProducingChanged);
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
            if (isProducing)
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