#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay.Messages
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