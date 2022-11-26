#nullable enable
using System.Collections.Generic;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace NovemberProject.CoreGameplay
{
    public sealed class IdleFolkSpawner : InitializableBehaviour
    {
        private const int MAX_NUMBER_OF_SAMPLING_TRIES = 100;
        private readonly List<IdleFolk> _spawnedIdleFolk = new();

        [SerializeField]
        private float _minimalWalkDistance = 1f;

        [SerializeField]
        private float _maximumWalkDistance = 25f;

        [SerializeField]
        private IdleFolk _idleFolkPrefab = null!;

        [SerializeField]
        private Transform[] _startingPoints = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Game.Instance.FolkManager.IdleFolk
                .TakeUntilDisable(this)
                .Subscribe(OnIdleFolkCountChanged);
        }

        public Vector3 GetNextDestination(Transform agentTransform)
        {
            Vector3 position = agentTransform.position;
            for (var tryNumber = 0; tryNumber < MAX_NUMBER_OF_SAMPLING_TRIES; tryNumber++)
            {
                Vector3 point = position +
                                Random.onUnitSphere * Random.Range(_minimalWalkDistance, _maximumWalkDistance);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(point, out hit, _maximumWalkDistance, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            Debug.LogError($"Could not find point to walk in {MAX_NUMBER_OF_SAMPLING_TRIES} tries.",
                agentTransform.gameObject);
            return transform.position;
        }

        private void OnIdleFolkCountChanged(int idleFolkCount)
        {
            if (_spawnedIdleFolk.Count == idleFolkCount)
            {
                return;
            }

            if (_spawnedIdleFolk.Count > idleFolkCount)
            {
                for (int i = _spawnedIdleFolk.Count; i > idleFolkCount; i--)
                {
                    IdleFolk? folk = _spawnedIdleFolk[i - 1];
                    _spawnedIdleFolk.Remove(folk);
                    Destroy(folk.gameObject);
                }

                return;
            }

            for (int i = _spawnedIdleFolk.Count ; i < idleFolkCount; i++)
            {
                Vector3 startingPoint = _startingPoints[Random.Range(0, _startingPoints.Length)].position;
                IdleFolk? folk = Instantiate(_idleFolkPrefab, startingPoint, Quaternion.identity, transform);
                _spawnedIdleFolk.Add(folk);
            }
        }
    }
}