#nullable enable
namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyStarvedMessage : INotificationMessage
    {
        public readonly int Count;

        public ArmyStarvedMessage(int count)
        {
            Count = count;
        }
    }
}