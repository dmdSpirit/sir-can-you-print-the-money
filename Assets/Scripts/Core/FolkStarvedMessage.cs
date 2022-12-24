#nullable enable
namespace NovemberProject.Core
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