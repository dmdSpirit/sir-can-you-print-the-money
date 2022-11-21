#nullable enable
using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace NovemberProject.MovingResources
{
    public sealed class MoveEffect
    {
        private readonly Subject<MoveEffect> _onFinished = new();
        private readonly Subject<MoveEffect> _onReadyToDestroy = new();

        private readonly Vector3 _start;
        private readonly Vector3 _finish;
        private readonly float _time;

        private Ease _ease = Ease.Linear;

        public GameObject MovingObject { get; }
        public IObservable<MoveEffect> OnFinished => _onFinished;
        public IObservable<MoveEffect> OnReadyToDestroy => _onReadyToDestroy;

        // ReSharper disable once TooManyDependencies
        public MoveEffect(GameObject movingObject, Vector3 start, Vector3 finish, float time = 1f)
        {
            MovingObject = movingObject;
            _start = start;
            _finish = finish;
            _time = time;
        }

        public void SetEase(Ease ease) => _ease = ease;

        public void Start()
        {
            MovingObject.transform.position = _start;
            Vector3 medianPoint = (_start + _finish) / 2;
            medianPoint.y += 1;
            MovingObject.transform.DOPath(new[] { _start, medianPoint, _finish }, _time, PathType.CatmullRom)
                .SetEase(_ease)
                .OnComplete(MoveFinished);
        }

        private void MoveFinished()
        {
            _onFinished.OnNext(this);
            _onReadyToDestroy.OnNext(this);
        }
    }
}