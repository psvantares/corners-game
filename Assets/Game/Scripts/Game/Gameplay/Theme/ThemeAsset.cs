using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Theme
{
    [CreateAssetMenu(fileName = "ThemeAsset", menuName = "ScriptableObject/ThemeAsset", order = 1)]
    public class ThemeAsset : ScriptableObject
    {
        [SerializeField]
        private Theme[] theme;

        public IEnumerable<Theme> Theme => theme;
    }

    public enum ThemeStyle
    {
        Default,
        Theme1,
        Theme2
    }

    public enum ThemeColor
    {
        ColorA,
        ColorB,
        ColorC,
        ColorD,
        ColorE,
        ColorF,
        ColorG,
        ColorH
    }

    [System.Serializable]
    public class Theme
    {
        [field: SerializeField]
        public ThemeStyle ThemeStyle;

        [field: SerializeField]
        public Color ColorA;

        [field: SerializeField]
        public Color ColorB;

        [field: SerializeField]
        public Color ColorC;

        [field: SerializeField]
        public Color ColorD;

        [field: SerializeField]
        public Color ColorE;

        [field: SerializeField]
        public Color ColorF;

        [field: SerializeField]
        public Color ColorG;

        [field: SerializeField]
        public Color ColorH;
    }
}