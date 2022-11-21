#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay.Messages
{
    public sealed class ArmyDesertedMessage : IMessage
    {
        public readonly int NumberOfDesertedArmy;

        public ArmyDesertedMessage(int numberOfDesertedArmy)
        {
            NumberOfDesertedArmy = numberOfDesertedArmy;
        }
    }
}