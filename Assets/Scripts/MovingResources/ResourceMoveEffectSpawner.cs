#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace NovemberProject.MovingResources
{
    public sealed class ResourceMoveEffectSpawner
    {
        private readonly List<MoveEffect> _moveEffects = new();
        private readonly Subject<Unit> _onEffectsFinished = new();

        private readonly ResourceMoveEffectSpawnerSettings _settings;
        private readonly TimeSystem _timeSystem;
        private readonly ResourceObjectFactory _resourceObjectFactory;

        public IReadOnlyList<MoveEffect> MoveEffects => _moveEffects;
        public IObservable<Unit> OnEffectsFinished => _onEffectsFinished;

        public ResourceMoveEffectSpawner(ResourceMoveEffectSpawnerSettings resourceMoveEffectSpawnerSettings,
            TimeSystem timeSystem, ResourceObjectFactory resourceObjectFactory)
        {
            _settings = resourceMoveEffectSpawnerSettings;
            _timeSystem = timeSystem;
            _resourceObjectFactory = resourceObjectFactory;
        }

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
            start.y += _settings.Height;
            finish.y += _settings.Height;
            var effect = new MoveEffect(_timeSystem, movingObject, start, finish, _settings.MoveDuration);
            _moveEffects.Add(effect);
            effect.SetEase(_settings.MovingEase);
            effect.OnReadyToDestroy.Subscribe(OnEffectReadyToDestroy);
            effect.Start();
            return effect;
        }

        private void OnEffectReadyToDestroy(MoveEffect effect)
        {
            Assert.IsTrue(_moveEffects.Contains(effect));
            _moveEffects.Remove(effect);
            Object.Destroy(effect.MovingObject);
            if (_moveEffects.Count == 0)
            {
                _onEffectsFinished.OnNext(Unit.Default);
            }
        }
    }
}