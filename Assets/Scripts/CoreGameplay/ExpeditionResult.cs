#nullable enable
namespace NovemberProject.CoreGameplay
{
    public readonly struct ExpeditionResult
    {
        public readonly int Explorers;
        public readonly int Rewards;
        public readonly int Defenders;
        public readonly bool IsSuccess;

        public ExpeditionResult(int explorers, int rewards, bool isSuccess, int defenders)
        {
            Explorers = explorers;
            Rewards = rewards;
            IsSuccess = isSuccess;
            Defenders = defenders;
        }
    }
}