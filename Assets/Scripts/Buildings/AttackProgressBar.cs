#nullable enable
using System;
using NovemberProject.Core;
using NovemberProject.Time;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class AttackProgressBar : MonoBehaviour
    {
        private IDisposable? _productionSub;

        private CombatController _combatController = null!;

        [SerializeField]
        private Image _progressImage = null!;

        [SerializeField]
        private GameObject _progressBar = null!;

        [Inject]
        private void Construct(CombatController combatController)
        {
            _combatController = combatController;
        }

        private void Update()
        {
            IReadOnlyTimer? timer = _combatController.AttackTimer;
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