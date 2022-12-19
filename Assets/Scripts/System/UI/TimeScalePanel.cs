#nullable enable
using System;
using System.Globalization;
using NovemberProject.CommonUIStuff;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.System.UI
{
    public sealed class TimeScalePanel : UIElement<object?>
    {
        private TimeSystem _timeSystem = null!;
        private IDisposable? _sub;

        [SerializeField]
        private TMP_Text _timeScale = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnShow(object? _)
        {
            _sub = _timeSystem.TimeScale.Subscribe(UpdateScale);
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void UpdateScale(float timeScale)
        {
            _timeScale.text = timeScale.ToString(CultureInfo.InvariantCulture);
        }
    }
}