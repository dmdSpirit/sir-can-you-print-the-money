#nullable enable
namespace NovemberProject.CoreGameplay
{
    public sealed class FolkExecutedMessage : INotificationMessage
    {
        public readonly int Count;

        public FolkExecutedMessage(int count)
        {
            Count = count;
        }
    }
}