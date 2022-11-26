#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace NovemberProject.CoreGameplay
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class IdleFolk : InitializableBehaviour
    {
        private const float STOP_DISTANCE = 0.01f;
        private NavMeshAgent _navMeshAgent = null!;
        private Timer? _waitingTimer;
        private bool _isWaiting;

        [SerializeField]
        private float _speed = 10f;

        [SerializeField]
        private float _minimalThinkingTime = 1f;

        [SerializeField]
        private float _maximumThinkingTime = 5f;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _speed;
            Game.Instance.TimeSystem.TimeScale
                .TakeUntilDisable(this)
                .Subscribe(OnTimeScaleChanged);
            StartWalking();
        }

        private void OnDestroy()
        {
            _waitingTimer?.Cancel();
        }

        private void WaitAndThink()
        {
            _isWaiting = true;
            float thinkingTime = Random.Range(_minimalThinkingTime, _maximumThinkingTime);
            _waitingTimer = Game.Instance.TimeSystem.CreateTimer(thinkingTime, _ => StartWalking());
            _waitingTimer.Start();
        }

        private void OnTimeScaleChanged(float timeScale)
        {
            _navMeshAgent.speed = _speed * timeScale;
        }

        private void StartWalking()
        {
            _navMeshAgent.destination = GetNextDestination();
            _isWaiting = false;
        }

        private Vector3 GetNextDestination()
        {
            return Game.Instance.IdleFolkSpawner.GetNextDestination(transform);
        }

        private void Update()
        {
            if (_isWaiting || _navMeshAgent.pathPending || _navMeshAgent.remainingDistance > STOP_DISTANCE)
            {
                return;
            }

            WaitAndThink();
        }
    }
}