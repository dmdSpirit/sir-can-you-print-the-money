#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.Input;
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
        private readonly TechTreeState _techTreeState;

        private readonly Subject<State> _onStateChanged = new();

        private readonly InputSystem _inputSystem;
        private readonly MessageBroker _messageBroker;

        private ExpeditionFinishedState? _expeditionFinishedState;
        private AttackState? _attackState;
        private State? _currentState;

        public IObservable<State> OnStateChanged => _onStateChanged;
        public State CurrentState => _currentState;

        public GameStateMachine(InputSystem inputSystem, MessageBroker messageBroker)
        {
            _inputSystem = inputSystem;
            _messageBroker = messageBroker;
            _exitGameState = new ExitGameState();
            _mainMenuState = new MainMenuState();
            _mainMenuState.AddInputHandler(_inputSystem.GetInputHandler<EscapeToExitGameHandler>());
            _newGameState = new NewGameState(this, _messageBroker);
            _roundState = new RoundState();
            _roundState.AddInputHandler(_inputSystem.GetInputHandler<MoveCameraHandler>());
            _roundState.AddInputHandler(_inputSystem.GetInputHandler<TimeControlsHandler>());
            _roundState.AddInputHandler(_inputSystem.GetInputHandler<MouseSelectionHandler>());
            _roundEndState = new RoundEndState(this);
            _roundEndState.AddInputHandler(_inputSystem.GetInputHandler<EscapeToMainMenuHandler>());
            _initializeGameState = new InitializeGameState(_inputSystem, this);
            _roundStartState = new RoundStartState();
            _gameOverState = new GameOverState();
            _victoryState = new VictoryState();
            _creditsScreenState = new CreditsScreenState();
            _tutorialState = new TutorialState();
            _techTreeState = new TechTreeState();

            _inputSystem.OnHandleInput.Subscribe(HandleInput);
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
        public void ShowTechTree() => _techTreeState.Enter();
        public void HideTechTree() => _techTreeState.Exit();

        public void ShowAttackResult(AttackData attackData)
        {
            _attackState = new AttackState(attackData);
            _attackState.Enter();
        }

        public void HideAttackResult() => _attackState?.Exit();

        private void ChangeState(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
            _onStateChanged.OnNext(_currentState);
        }

        private void HandleInput(Unit _)
        {
            _currentState?.HandleInput();
        }
    }
}