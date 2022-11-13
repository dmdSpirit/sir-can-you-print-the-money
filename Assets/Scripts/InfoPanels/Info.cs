#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.InfoPanels
{
    [Serializable]
    public sealed class Info : IInfo
    {
        private Subject<Unit> _onUpdated = new();

        public Sprite Image { get; private set; }
        public string Text { get; private set; }
        public IObservable<Unit> OnUpdated => _onUpdated;

        public Info(Sprite image, string text)
        {
            Image = image;
            Text = text;
        }

        public void UpdateText(string text)
        {
            Text = text;
            _onUpdated.OnNext(Unit.Default);
        }
        
        public void UpdateImage(Sprite image)
        {
            Image = image;
            _onUpdated.OnNext(Unit.Default);
        }
    }
}