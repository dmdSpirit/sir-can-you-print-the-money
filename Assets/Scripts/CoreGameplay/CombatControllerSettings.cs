#nullable enable
using UnityEngine;

namespace NovemberProject.CoreGameplay
{
    [CreateAssetMenu(menuName = "Create CombatControllerSettings", fileName = "CombatControllerSettings", order = 0)]
    public sealed class CombatControllerSettings : ScriptableObject
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

        [SerializeField]
        private int[] _attackersCount = null!;

        [SerializeField]
        private float _attackDuration = 57f;

        [SerializeField]
        private int _attacksFromWeek = 2;

        public float AttackDuration => _attackDuration;
        public int AttacksFromWeek => _attacksFromWeek;

        public int GetAttackersCount(int attackIndex)
        {
            int maxIndex = _attackersCount.Length - 1;
            if (attackIndex > maxIndex)
            {
                return _attackersCount[maxIndex];
            }

            return _attackersCount[attackIndex];
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
    }
}