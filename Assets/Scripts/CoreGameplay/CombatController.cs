﻿#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UnityEngine;
using UnityEngine.Assertions;

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

        private Timer? _attackTimer;
        private int _attackIndex;

        [SerializeField]
        private int[] _attackersCount = null!;

        [SerializeField]
        private float _attackDuration = 40f;

        public IReadOnlyTimer? AttackTimer => _attackTimer;

        public void InitializeGameData()
        {
            _attackIndex = 0;
            _attackTimer?.Cancel();
            PlanNextAttack();
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
            _attackTimer = Game.Instance.TimeSystem.CreateTimer(_attackDuration, OnAttack);
            _attackTimer.Start();
        }

        private void OnAttack(Timer _)
        {
            int defenders = Game.Instance.ArmyManager.GuardsCount.Value;
            int attackers = NextAttackersCount();
            bool attackersWon = CalculateAttackResult(attackers, defenders);
            AttackStatus attackStatus = AttackStatus.DefendersWon;
            if (attackersWon)
            {
                if (defenders > 0)
                {
                    Game.Instance.ArmyManager.KillGuards();
                    attackStatus = AttackStatus.GuardsKilled;
                }
                else if (Game.Instance.FolkManager.FolkCount.Value > 0)
                {
                    attackStatus = AttackStatus.FolkKilled;
                    Game.Instance.FolkManager.KillFolk(1);
                }
            }

            Game.Instance.GameStateMachine.ShowAttackResult(new AttackData(defenders, attackers, attackStatus));
            PlanNextAttack();
        }

        public bool CalculateAttackResult(int attackers, int defenders)
        {
            float probability = GetWinProbability(attackers, defenders);
            float roll = Random.value;
            return roll <= probability;
        }

        private float GetWinProbability(int attackers, int defenders)
        {
            Assert.IsTrue(attackers <= MAX_ATTACKERS);
            Assert.IsTrue(defenders <= MAX_DEFENDERS);
            return _winProbability[defenders, attackers];
        }
    }
}