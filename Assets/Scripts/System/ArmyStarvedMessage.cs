#nullable enable
namespace NovemberProject.System
{
    public sealed class ArmyStarvedMessage : IMessage
    {
        public readonly int NumberOfStarvedArmy;

        public ArmyStarvedMessage(int numberOfStarvedArmy)
        {
            NumberOfStarvedArmy = numberOfStarvedArmy;
        }
    }
}