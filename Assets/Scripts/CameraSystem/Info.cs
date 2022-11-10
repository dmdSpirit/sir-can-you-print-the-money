#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    [Serializable]
    public class Info:IInfo
    {
        private Subject<Unit> _onUpdated = new();

        public Sprite Image { get; }
        public string Text { get; }
        public IObservable<Unit> OnUpdated => _onUpdated;
    }
}