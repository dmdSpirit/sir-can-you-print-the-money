#nullable enable
namespace NovemberProject.System
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