#nullable enable
using System;
using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public sealed class FPSPanel : UIElement<object?>
    {
        private IDisposable? _sub;

        [SerializeField]
        private TMP_Text _current = null!;

        [SerializeField]
        private TMP_Text _average = null!;

        protected override void OnShow(object? value)
        {
            _sub?.Dispose();
            _sub = Observable.Timer(TimeSpan.FromSeconds(.3)).Repeat().Subscribe(UpdateFPS);
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void UpdateFPS(long _)
        {
            _current.text = (1f / UnityEngine.Time.unscaledDeltaTime).ToString(CultureInfo.InvariantCulture);
            _average.text =
                (UnityEngine.Time.frameCount / UnityEngine.Time.unscaledTime).ToString(CultureInfo.InvariantCulture);
        }
    }
}