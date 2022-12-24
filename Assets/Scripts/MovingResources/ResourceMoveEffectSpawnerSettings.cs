#nullable enable
using DG.Tweening;
using UnityEngine;

namespace NovemberProject.MovingResources
{
    [CreateAssetMenu(menuName = "Create ResourceMoveEffectSpawnerSettings", fileName = "ResourceMoveEffectSpawnerSettings", order = 0)]
    public sealed class ResourceMoveEffectSpawnerSettings : ScriptableObject
    {
        [SerializeField]
        private float _height;

        [SerializeField]
        private float _moveDuration = 1.5f;

        [SerializeField]
        private Ease _movingEase = Ease.InOutQuad;

        public float Height => _height;
        public float MoveDuration => _moveDuration;
        public Ease MovingEase => _movingEase;
    }
}