#nullable enable
namespace NovemberProject.Core
{
    public readonly struct AttackData
    {
        public readonly int Defenders;
        public readonly int Attackers;
        public readonly AttackStatus AttackStatus;

        public AttackData(int defenders, int attackers, AttackStatus attackStatus)
        {
            Defenders = defenders;
            Attackers = attackers;
            AttackStatus = attackStatus;
        }
    }
}