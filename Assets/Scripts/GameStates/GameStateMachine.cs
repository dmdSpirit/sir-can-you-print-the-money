#nullable enable
using System;
using NovemberProject.Input;
using NovemberProject.System;
using UniRx;

namespace NovemberProject.GameStates
{
    public class GameStateMachine
    {
        private readonly ExitGameState _exitGameState;
        private readonly MainMenuState _mainMenuState;
        private readonly NewGameState _newGameState;
        private readonly TurnState _turnState;
        private readonly EndOnTurnState _endOnTurnState;

        private readonly Subject<State> _onStateChanged = new();

        private State? _currentState;

        public IObservable<State> OnStateChanged => _onStateChanged;

        public GameStateMachine()
        {
            InputSystem inputSystem = Game.Instance.InputSystem;
            _exitGameState = new ExitGameState();
            _mainMenuState = new MainMenuState();
            _mainMenuState.AddInputHandler(inputSystem.GetInputHandler<EscapeToExitGameHandler>());
            _newGameState = new NewGameState();
            _turnState = new TurnState();
            _turnState.AddInputHandler(inputSystem.GetInputHandler<EscapeToMainMenuHandler>());
            _endOnTurnState = new EndOnTurnState();
            _endOnTurnState.AddInputHandler(inputSystem.GetInputHandler<EscapeToMainMenuHandler>());

            Game.Instance.InputSystem.OnHandleInput.Subscribe(_ => HandleInput());
        }

        public void NewGame() => ChangeState(_newGameState);
        public void ExitGame() => ChangeState(_exitGameState);
        public void MainMenu() => ChangeState(_mainMenuState);
        public void Turn() => ChangeState(_turnState);
        public void EndTurn() => ChangeState(_endOnTurnState);

        private void ChangeState(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
            _onStateChanged.OnNext(_currentState);
        }

        private void HandleInput()
        {
            _currentState?.HandleInput();
        }
    }
}