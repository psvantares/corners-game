using Game.Core;

namespace Game.Services
{
    public class LoaderService : IGameService
    {
        private readonly LoaderManager loaderManager;

        public LoaderService(LoaderManager loaderManager)
        {
            this.loaderManager = loaderManager;
        }

        public void Show()
        {
            loaderManager.Show();
        }

        public void Hide()
        {
            loaderManager.Hide();
        }
    }
}