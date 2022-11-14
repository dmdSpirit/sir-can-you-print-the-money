#nullable enable
using System.Collections.Generic;
using DG.Tweening;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Money
{
    public sealed class ResourceMoveEffectSpawner : InitializableBehaviour
    {
        private readonly List<MoveEffect> _moveEffects = new();

        [SerializeField]
        private ResourceObjectFactory _resourceObjectFactory = null!;

        [SerializeField]
        private Ease _movingEase;

        public MoveEffect ShowMovingCoin(Vector3 start, Vector3 finish, float time)
        {
            var effect = new MoveEffect(_resourceObjectFactory.Coin(), start, finish, time);
            _moveEffects.Add(effect);
            effect.SetEase(_movingEase);
            effect.OnFinished.Subscribe(OnEffectFinished);
            effect.Start();
            return effect;
        }

        private void OnEffectFinished(MoveEffect effect)
        {
            Assert.IsTrue(_moveEffects.Contains(effect));
            _moveEffects.Remove(effect);
            Destroy(effect.MovingObject);
        }
    }
}