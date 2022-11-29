#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.InputSystem;
using NovemberProject.System;
using UniRx;

namespace NovemberProject.GameStates
{
    public sealed class GameStateMachine
    {
        private readonly ExitGameState _exitGameState;
        private readonly MainMenuState _mainMenuState;
        private readonly NewGameState _newGameState;
        private readonly RoundState _roundState;
        private readonly RoundEndState _roundEndState;
        private readonly RoundStartState _roundStartState;
        private readonly InitializeGameState _initializeGameState;
        private readonly GameOverState _gameOverState;
        private readonly VictoryState _victoryState;
        private readonly CreditsScreenState _creditsScreenState;
        private readonly TutorialState _tutorialState;

        private readonly Subject<State> _onStateChanged = new();

        private ExpeditionFinishedState? _expeditionFinishedState;
        private State? _currentState;

        public IObservable<State> OnStateChanged => _onStateChanged;

        public GameStateMachine()
        {
            InputSystem.InputSystem inputSystem = Game.Instance.InputSystem;
            _exitGameState = new ExitGameState();
            _mainMenuState = new MainMenuState();
            _mainMenuState.AddInputHandler(inputSystem.GetInputHandler<EscapeToExitGameHandler>());
            _newGameState = new NewGameState();
            _roundState = new RoundState();
            _roundState.AddInputHandler(inputSystem.GetInputHandler<EscapeToMainMenuHandler>());
            _roundState.AddInputHandler(inputSystem.GetInputHandler<MoveCameraHandler>());
            _roundState.AddInputHandler(inputSystem.GetInputHandler<TimeControlsHandler>());
            _roundState.AddInputHandler(inputSystem.GetInputHandler<MouseSelectionHandler>());
            _roundEndState = new RoundEndState();
            _roundEndState.AddInputHandler(inputSystem.GetInputHandler<EscapeToMainMenuHandler>());
            _initializeGameState = new InitializeGameState();
            _roundStartState = new RoundStartState();
            _gameOverState = new GameOverState();
            _victoryState = new VictoryState();
            _creditsScreenState = new CreditsScreenState();
            _tutorialState = new TutorialState();

            Game.Instance.InputSystem.OnHandleInput.Subscribe(_ => HandleInput());
        }

        public void NewGame() => ChangeState(_newGameState);
        public void ExitGame() => ChangeState(_exitGameState);
        public void MainMenu() => ChangeState(_mainMenuState);
        public void Round() => ChangeState(_roundState);
        public void InitializeGame() => ChangeState(_initializeGameState);
        public void FinishRound() => ChangeState(_roundEndState);
        public void StartRound() => ChangeState(_roundStartState);
        public void GameOver() => ChangeState(_gameOverState);
        public void Victory() => ChangeState(_victoryState);
        public void Credits() => ChangeState(_creditsScreenState);
        public void Tutorial() => ChangeState(_tutorialState);

        // FIXME (Stas): Ugly
        public void ExpeditionFinished(ExpeditionResult expeditionResult)
        {
            _expeditionFinishedState = new ExpeditionFinishedState(expeditionResult);
            _expeditionFinishedState.Enter();
        }

        public void ExpeditionFinishedExit() => _expeditionFinishedState?.Exit();

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