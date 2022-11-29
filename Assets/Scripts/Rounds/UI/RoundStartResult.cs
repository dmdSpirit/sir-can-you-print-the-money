#nullable enable
namespace NovemberProject.Rounds.UI
{
    public sealed class RoundStartResult
    {
        public int ArmyDeserted;

        public void Reset()
        {
            ArmyDeserted = 0;
        }
    }
}