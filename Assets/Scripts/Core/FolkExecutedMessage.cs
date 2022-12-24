#nullable enable
namespace NovemberProject.Core
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