#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.GameStates;
using NovemberProject.Rounds;
using NovemberProject.System;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;

namespace NovemberProject.CoreGameplay
{
    public sealed class CombatController : InitializableBehaviour
    {
        private const int MAX_ATTACKERS = 7;
        private const int MAX_DEFENDERS = 9;

        private readonly float[,] _winProbability =
        {
            { .42f, .75f, .92f, .97f, .99f, 1, 1 },
            { .11f, .36f, .66f, .79f, .89f, .93f, .97f },
            { .03f, .21f, .47f, .64f, .77f, .86f, .91f },
            { .01f, .09f, .31f, .48f, .64f, .74f, .83f },
            { 0f, .05f, .21f, .36f, .51f, .64f, .74f },
            { 0f, .02f, .13f, .25f, .4f, .52f, .64f },
            { 0f, .01f, .08f, .18f, .3f, .42f, .54f },
            { 0f, 0f, .05f, .12f, .22f, .33f, .45f },
            { 0f, 0f, .03f, .09f, .16f, .26f, .36f }
        };

        private readonly Subject<Unit> _onNewAttack = new();
        private readonly ReactiveProperty<bool> _isActive = new();

        private FolkManager _folkManager = null!;
        private ArmyManager _armyManager = null!;
        private GameStateMachine _gameStateMachine = null!;
        private TimeSystem _timeSystem = null!;
        private RoundSystem _roundSystem = null!;
        private MessageBroker _messageBroker = null!;

        private Timer? _attackTimer;
        private int _attackIndex;

        [SerializeField]
        private int[] _attackersCount = null!;

        [SerializeField]
        private float _attackDuration = 40f;

        [SerializeField]
        private int _attacksFromWeek = 2;

        public IReadOnlyTimer? AttackTimer => _attackTimer;
        public IObservable<Unit> OnNewAttack => _onNewAttack;
        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

        [Inject]
        private void Construct(FolkManager folkManager, ArmyManager armyManager, GameStateMachine gameStateMachine,
            TimeSystem timeSystem, RoundSystem roundSystem, MessageBroker messageBroker)
        {
            _folkManager = folkManager;
            _armyManager = armyManager;
            _gameStateMachine = gameStateMachine;
            _timeSystem = timeSystem;
            _roundSystem = roundSystem;
            _messageBroker = messageBroker;
            _roundSystem.Round.Subscribe(OnRoundChanged);
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _attackIndex = 0;
            _attackTimer?.Cancel();
            _isActive.Value = false;
        }

        public int NextAttackersCount()
        {
            int maxAttackIndex = _attackersCount.Length - 1;
            if (_attackIndex < maxAttackIndex)
            {
                return _attackersCount[_attackIndex];
            }

            return _attackersCount[maxAttackIndex];
        }

        public void PlanNextAttack()
        {
            _attackTimer = _timeSystem.CreateTimer(_attackDuration, OnAttack);
            _attackTimer.Start();
            _onNewAttack.OnNext(Unit.Default);
        }

        private void OnAttack(Timer _)
        {
            int defenders = _armyManager.GuardsCount.Value;
            int attackers = NextAttackersCount();
            bool attackersWon = CalculateAttackResult(attackers, defenders);
            AttackStatus attackStatus = AttackStatus.DefendersWon;
            if (attackersWon)
            {
                if (defenders > 0)
                {
                    _armyManager.KillGuards(attackers);
                    attackStatus = AttackStatus.GuardsKilled;
                }
                else if (_folkManager.FolkCount.Value > 0)
                {
                    attackStatus = AttackStatus.FolkKilled;
                    _folkManager.KillFolk(1);
                }

                _attackIndex = 0;
            }
            else
            {
                _attackIndex++;
            }

            _gameStateMachine.ShowAttackResult(new AttackData(defenders, attackers, attackStatus));
            PlanNextAttack();
        }

        public bool CalculateAttackResult(int attackers, int defenders)
        {
            float probability = GetAttackersWinProbability(attackers, defenders);
            float roll = Random.value;
            return roll <= probability;
        }

        public float GetAttackersWinProbability(int attackers, int defenders)
        {
            if (defenders == 0)
            {
                return 1;
            }

            if (attackers == 0)
            {
                return 0;
            }

            return _winProbability[Mathf.Min(defenders, MAX_DEFENDERS), Mathf.Min(attackers, MAX_ATTACKERS)];
        }

        private void OnRoundChanged(int round)
        {
            _isActive.Value = round >= _attacksFromWeek;
            if (round == _attacksFromWeek)
            {
                PlanNextAttack();
            }
        }
    }
}