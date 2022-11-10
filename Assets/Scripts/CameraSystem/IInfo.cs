#nullable enable
using System;
using UniRx;
using UnityEngine;

namespace NovemberProject.CameraSystem
{
    public interface IInfo
    {
        Sprite Image { get; }
        string Text { get; }
        IObservable<Unit> OnUpdated { get; }
    }
}