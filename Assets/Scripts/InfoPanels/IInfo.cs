#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.InfoPanels
{
    public interface IInfo
    {
        Sprite Image { get; }
        string Text { get; }
        IObservable<Unit> OnUpdated { get; }
        void UpdateText(string text);
        void UpdateImage(Sprite image);
    }
}