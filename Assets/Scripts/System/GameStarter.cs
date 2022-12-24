#nullable enable
using NovemberProject.GameStates;
using UnityEngine;
using Zenject;

namespace NovemberProject.System
{
    public sealed class GameStarter : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _gameStateMachine.InitializeGame();
        }
    }
}