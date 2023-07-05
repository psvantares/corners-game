using Game.Core;

namespace Game.Services
{
    public class ThemeService : IGameService
    {
        private readonly ThemeManager themeManager;

        public ThemeService(ThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }
    }
}