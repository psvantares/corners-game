using Game.Utilities;

namespace Game.Services
{
    public class ThemeController : IThemeController
    {
        private readonly SavableValue<int> themeStyle = new("ThemeController.themeStyle");

        int IThemeController.ThemeStyle
        {
            get => themeStyle.Value;
            set
            {
                if (themeStyle.Value == value)
                {
                    return;
                }

                themeStyle.Value = value;
            }
        }
    }
}