#nullable enable
namespace NovemberProject.CommonUIStuff
{
    public interface IUIScreen
    {
        public bool IsShown { get; }
        public void Show();
        public void Hide();
    }
}