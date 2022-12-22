#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public interface IVictoryScreen : IUIScreen{}
    
    public sealed class VictoryScreen : UIScreen, IVictoryScreen
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _toMainMenuButton = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _toMainMenuButton.OnClickAsObservable()
                .Subscribe(OnToMainMenuButtonClicked);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        private void OnToMainMenuButtonClicked(Unit _)
        {
            _gameStateMachine.MainMenu();
        }
    }
}