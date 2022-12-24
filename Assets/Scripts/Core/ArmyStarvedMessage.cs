#nullable enable
namespace NovemberProject.Core
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