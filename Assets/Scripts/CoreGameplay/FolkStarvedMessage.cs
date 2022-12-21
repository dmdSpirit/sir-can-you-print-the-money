#nullable enable
namespace NovemberProject.CoreGameplay
{
    public sealed class FolkStarvedMessage : INotificationMessage
    {
        public readonly int Count;

        public FolkStarvedMessage(int count)
        {
            Count = count;
        }
    }
}