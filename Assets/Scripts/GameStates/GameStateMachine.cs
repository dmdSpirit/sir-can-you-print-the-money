#nullable enable
using System;
using NovemberProject.CoreGameplay;
using NovemberProject.Input;
using NovemberProject.Rounds;
using NovemberProject.System;
using NovemberProject.Time;
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
        private readonly TimeSystem _timeSystem;
        private readonly RoundSystem _roundSystem;

        private ExpeditionFinishedState? _expeditionFinishedState;
        private AttackState? _attackState;
        private State? _currentState;

        public IObservable<State> OnStateChanged => _onStateChanged;
        public State CurrentState => _currentState;

        public GameStateMachine(InputSystem inputSystem, TimeSystem timeSystem, RoundSystem roundSystem,
            MessageBroker messageBroker)
        {
            _inputSystem = inputSystem;
            _messageBroker = messageBroker;
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
            _exitGameState = new ExitGameState();
            _mainMenuState = new MainMenuState(_timeSystem);
            _mainMenuState.AddInputHandler(_inputSystem.GetInputHandler<EscapeToExitGameHandler>());
            _newGameState = new NewGameState(this, _timeSystem, _messageBroker);
            _roundState = new RoundState(_timeSystem, _roundSystem);
            _roundState.AddInputHandler(_inputSystem.GetInputHandler<MoveCameraHandler>());
            _roundState.AddInputHandler(_inputSystem.GetTimeControlsHandler(_timeSystem));
            _roundState.AddInputHandler(_inputSystem.GetInputHandler<MouseSelectionHandler>());
            _roundEndState = new RoundEndState(this, _timeSystem, _roundSystem);
            _initializeGameState = new InitializeGameState(_inputSystem, this, _timeSystem);
            _roundStartState = new RoundStartState(_roundSystem);
            _gameOverState = new GameOverState();
            _victoryState = new VictoryState(_timeSystem);
            _creditsScreenState = new CreditsScreenState();
            _tutorialState = new TutorialState();
            _techTreeState = new TechTreeState(_timeSystem);

            _inputSystem.OnHandleInput.Subscribe(HandleInput);
            _roundSystem.OnRoundEnded.Subscribe(FinishRound);
        }

        public void NewGame() => ChangeState(_newGameState);
        public void ExitGame() => ChangeState(_exitGameState);
        public void MainMenu() => ChangeState(_mainMenuState);
        public void Round() => ChangeState(_roundState);
        public void InitializeGame() => ChangeState(_initializeGameState);
        public void FinishRound(Unit _) => ChangeState(_roundEndState);
        public void StartRound() => ChangeState(_roundStartState);
        public void GameOver() => ChangeState(_gameOverState);
        public void Victory() => ChangeState(_victoryState);
        public void Credits() => ChangeState(_creditsScreenState);
        public void Tutorial() => ChangeState(_tutorialState);

        // FIXME (Stas): Ugly
        public void ExpeditionFinished(ExpeditionResult expeditionResult)
        {
            _expeditionFinishedState = new ExpeditionFinishedState(expeditionResult, _timeSystem);
            _expeditionFinishedState.Enter();
        }

        public void ExpeditionFinishedExit() => _expeditionFinishedState?.Exit();
        public void ShowTechTree() => _techTreeState.Enter();
        public void HideTechTree() => _techTreeState.Exit();

        public void ShowAttackResult(AttackData attackData)
        {
            _attackState = new AttackState(attackData, _timeSystem);
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