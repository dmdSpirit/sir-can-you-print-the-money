#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings
{
    public sealed class AttackProgressBar : InitializableBehaviour
    {
        private IDisposable? _productionSub;

        [SerializeField]
        private Image _progressImage = null!;

        [SerializeField]
        private GameObject _progressBar = null!;

        private void Update()
        {
            IReadOnlyTimer? timer = Game.Instance.CombatController.AttackTimer;
            if (timer == null)
            {
                _progressBar.gameObject.SetActive(false);
                return;
            }
            _progressBar.gameObject.SetActive(true);
            _progressImage.fillAmount = 1 - timer.ProgressRate;
        }
    }
}