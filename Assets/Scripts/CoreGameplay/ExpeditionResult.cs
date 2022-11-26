#nullable enable
namespace NovemberProject.CoreGameplay
{
    public readonly struct ExpeditionResult
    {
        public readonly int Explorers;
        public readonly int Rewards;

        public ExpeditionResult(int explorers, int rewards)
        {
            Explorers = explorers;
            Rewards = rewards;
        }
    }
}