using System;
using Game.Core;
using UnityEngine;

namespace Game.Services
{
    [ExecuteInEditMode]
    public class ThemeManager : MonoBehaviour, IGameService
    {
        [Header("THEME ASSET")]
        [SerializeField]
        private ThemeAsset themeAsset;

        [SerializeField]
        private ThemeStyle currentThemeStyle;

        public static event Action OnThemeEvent;

        public static int CurrentTheme => itc.ThemeStyle;
        private static Color ColorA { get; set; }
        private static Color ColorB { get; set; }
        private static Color ColorC { get; set; }
        private static Color ColorD { get; set; }
        private static Color ColorE { get; set; }
        private static Color ColorF { get; set; }
        private static Color ColorG { get; set; }
        private static Color ColorH { get; set; }

        private ThemeController themeController;
        private static IThemeController itc;

        private void Awake()
        {
            themeController = new ThemeController();
            itc = themeController;
            currentThemeStyle = (ThemeStyle)itc.ThemeStyle;

            Configure();
        }

#if UNITY_EDITOR
        private void OnValidate() => Configure();
#endif

        private void Configure()
        {
            foreach (var theme in themeAsset.Theme)
            {
                if (theme.ThemeStyle != currentThemeStyle)
                {
                    continue;
                }

                ColorA = theme.ColorA;
                ColorB = theme.ColorB;
                ColorC = theme.ColorC;
                ColorD = theme.ColorD;
                ColorE = theme.ColorE;
                ColorF = theme.ColorF;
                ColorG = theme.ColorG;
                ColorH = theme.ColorH;

                break;
            }

            OnThemeEvent?.Invoke();
        }

        public void SetTheme(int theme)
        {
            itc.ThemeStyle = theme;
            currentThemeStyle = (ThemeStyle)itc.ThemeStyle;

            Configure();
        }

        public static Color GetColor(ThemeColor themeColor)
        {
            return themeColor switch
            {
                ThemeColor.ColorA => ColorA,
                ThemeColor.ColorB => ColorB,
                ThemeColor.ColorC => ColorC,
                ThemeColor.ColorD => ColorD,
                ThemeColor.ColorE => ColorE,
                ThemeColor.ColorF => ColorF,
                ThemeColor.ColorG => ColorG,
                ThemeColor.ColorH => ColorH,
                _ => Color.white
            };
        }
    }
}