#nullable enable
namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyDesertedMessage : INotificationMessage
    {
        public readonly int Count;

        public ArmyDesertedMessage(int count)
        {
            Count = count;
        }
    }
}