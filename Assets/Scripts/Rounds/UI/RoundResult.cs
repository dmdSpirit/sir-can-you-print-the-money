#nullable enable
namespace NovemberProject.Rounds.UI
{
    public sealed class RoundResult
    {
        public int FolkExecuted;
        public int ArmyStarved;
        public int FolkStarved;
        public int ArmyDeserted;

        public void Reset()
        {
            FolkExecuted = 0;
            ArmyStarved = 0;
            FolkStarved = 0;
        }
    }
}