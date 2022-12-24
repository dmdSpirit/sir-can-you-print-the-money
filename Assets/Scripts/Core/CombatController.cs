#nullable enable
using System;
using NovemberProject.Core.FolkManagement;
using NovemberProject.GameStates;
using NovemberProject.Rounds;
using NovemberProject.System.Messages;
using NovemberProject.Time;
using UniRx;
using Random = UnityEngine.Random;

namespace NovemberProject.Core
{
    public sealed class CombatController
    {
        private readonly Subject<Unit> _onNewAttack = new();
        private readonly ReactiveProperty<bool> _isActive = new();

        private readonly CombatControllerSettings _settings;
        private readonly FolkManager _folkManager;
        private readonly ArmyManager _armyManager;
        private readonly GameStateMachine _gameStateMachine;
        private readonly TimeSystem _timeSystem;
        private readonly RoundSystem _roundSystem;
        private readonly MessageBroker _messageBroker;

        private Timer? _attackTimer;
        private int _attackIndex;

        public IReadOnlyTimer? AttackTimer => _attackTimer;
        public IObservable<Unit> OnNewAttack => _onNewAttack;
        public IReadOnlyReactiveProperty<bool> IsActive => _isActive;
        public int NextAttackersCount => _settings.GetAttackersCount(_attackIndex);

        public CombatController(CombatControllerSettings combatControllerSettings, FolkManager folkManager,
            ArmyManager armyManager, GameStateMachine gameStateMachine,
            TimeSystem timeSystem, RoundSystem roundSystem, MessageBroker messageBroker)
        {
            _settings = combatControllerSettings;
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

        public void PlanNextAttack()
        {
            _attackTimer = _timeSystem.CreateTimer(_settings.AttackDuration, OnAttack);
            _attackTimer.Start();
            _onNewAttack.OnNext(Unit.Default);
        }

        private void OnAttack(Timer _)
        {
            int defenders = _armyManager.GuardsCount.Value;
            int attackers = NextAttackersCount;
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

        public float GetAttackersWinProbability(int attackers, int defenders) =>
            _settings.GetAttackersWinProbability(attackers, defenders);


        private void OnRoundChanged(int round)
        {
            _isActive.Value = round >= _settings.AttacksFromWeek;
            if (round == _settings.AttacksFromWeek)
            {
                PlanNextAttack();
            }
        }
    }
}