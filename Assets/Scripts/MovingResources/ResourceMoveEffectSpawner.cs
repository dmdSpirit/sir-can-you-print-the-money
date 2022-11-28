#nullable enable
using System;
using System.Collections.Generic;
using DG.Tweening;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.MovingResources
{
    public sealed class ResourceMoveEffectSpawner : InitializableBehaviour
    {
        private readonly List<MoveEffect> _moveEffects = new();
        private readonly Subject<Unit> _onEffectsFinished = new();

        [SerializeField]
        private ResourceObjectFactory _resourceObjectFactory = null!;

        [SerializeField]
        private float _height;

        [SerializeField]
        private float _moveDuration = .75f;

        [SerializeField]
        private Ease _movingEase;

        public IReadOnlyList<MoveEffect> MoveEffects => _moveEffects;
        public IObservable<Unit> OnEffectsFinished => _onEffectsFinished;

        public MoveEffect ShowMovingStone(Vector3 start, Vector3 finish)
        {
            return ShowMovingEffect(start, finish, _resourceObjectFactory.Stone());
        }
        
        public MoveEffect ShowMovingCoin(Vector3 start, Vector3 finish)
        {
            return ShowMovingEffect(start, finish, _resourceObjectFactory.Coin());
        }

        public MoveEffect ShowMovingFood(Vector3 start, Vector3 finish)
        {
            return ShowMovingEffect(start, finish, _resourceObjectFactory.Food());
        }

        private MoveEffect ShowMovingEffect(Vector3 start, Vector3 finish, GameObject movingObject)
        {
            start.y += _height;
            finish.y += _height;
            var effect = new MoveEffect(movingObject, start, finish, _moveDuration);
            _moveEffects.Add(effect);
            effect.SetEase(_movingEase);
            effect.OnReadyToDestroy.Subscribe(OnEffectReadyToDestroy);
            effect.Start();
            return effect;
        }

        private void OnEffectReadyToDestroy(MoveEffect effect)
        {
            Assert.IsTrue(_moveEffects.Contains(effect));
            _moveEffects.Remove(effect);
            Destroy(effect.MovingObject);
            if (_moveEffects.Count == 0)
            {
                _onEffectsFinished.OnNext(Unit.Default);
            }
        }
    }
}