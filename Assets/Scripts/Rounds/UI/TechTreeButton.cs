#nullable enable
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class TechTreeButton : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _button = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _button.OnClickAsObservable()
                .Subscribe(OnButton);
        }

        private void OnButton(Unit _)
        {
            if (_gameStateMachine.CurrentState is not RoundState)
            {
                return;
            }

            _gameStateMachine.ShowTechTree();
        }
    }
}