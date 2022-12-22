#nullable enable
using System;
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.CoreGameplay;
using NovemberProject.Input;
using NovemberProject.MovingResources;
using NovemberProject.Rounds;
using NovemberProject.System.UI;
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
        private readonly CameraController _cameraController;
        private readonly UIManager _uiManager;
        private readonly BuildingSelector _buildingSelector;
        private readonly BuildingNameHover _buildingNameHover;
        private readonly CoreGameplay.CoreGameplay _coreGameplay;
        private readonly ResourceMoveEffectSpawner _resourceMoveEffectSpawner;

        private ExpeditionFinishedState? _expeditionFinishedState;
        private AttackState? _attackState;
        private State? _currentState;

        public IObservable<State> OnStateChanged => _onStateChanged;
        public State CurrentState => _currentState;

        public GameStateMachine(InputSystem inputSystem, TimeSystem timeSystem, RoundSystem roundSystem,
            CameraController cameraController,
            UIManager uiManager,
            BuildingSelector buildingSelector,
            BuildingNameHover buildingNameHover,
            CoreGameplay.CoreGameplay coreGameplay,
            ResourceMoveEffectSpawner resourceMoveEffectSpawner,
            MessageBroker messageBroker)
        {
            _inputSystem = inputSystem;
            _messageBroker = messageBroker;
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
            _cameraController = cameraController;
            _uiManager = uiManager;
            _buildingSelector = buildingSelector;
            _buildingNameHover = buildingNameHover;
            _coreGameplay = coreGameplay;
            _resourceMoveEffectSpawner = resourceMoveEffectSpawner;
            _exitGameState = new ExitGameState();
            _mainMenuState = new MainMenuState(_timeSystem, _cameraController, _uiManager);
            _mainMenuState.AddInputHandler(_inputSystem.GetInputHandler<EscapeToExitGameHandler>());
            _newGameState = new NewGameState(this, _timeSystem, _cameraController, _messageBroker);
            _roundState = new RoundState(_timeSystem, _roundSystem, _uiManager, _coreGameplay, _buildingSelector,
                buildingNameHover);
            _roundState.AddInputHandler(_inputSystem.GetMoveCameraHandler(_cameraController));
            _roundState.AddInputHandler(_inputSystem.GetTimeControlsHandler(_timeSystem));
            _roundState.AddInputHandler(
                _inputSystem.GetMouseSelectionHandler(_cameraController, _buildingNameHover, _buildingSelector));
            _roundEndState = new RoundEndState(this, _timeSystem, _roundSystem, _uiManager, _resourceMoveEffectSpawner,
                _coreGameplay);
            _initializeGameState =
                new InitializeGameState(_inputSystem, this, _timeSystem, _uiManager, _buildingNameHover);
            _roundStartState = new RoundStartState(_roundSystem, _uiManager);
            _gameOverState = new GameOverState(_uiManager);
            _victoryState = new VictoryState(_timeSystem, _uiManager, _buildingSelector);
            _creditsScreenState = new CreditsScreenState(_uiManager);
            _tutorialState = new TutorialState(_uiManager);
            _techTreeState = new TechTreeState(_timeSystem, _uiManager, _buildingSelector);

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
            _expeditionFinishedState =
                new ExpeditionFinishedState(expeditionResult, _timeSystem, _uiManager, _buildingSelector);
            _expeditionFinishedState.Enter();
        }

        public void ExpeditionFinishedExit() => _expeditionFinishedState?.Exit();
        public void ShowTechTree() => _techTreeState.Enter();
        public void HideTechTree() => _techTreeState.Exit();

        public void ShowAttackResult(AttackData attackData)
        {
            _attackState = new AttackState(attackData, _timeSystem, _uiManager, _buildingSelector);
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