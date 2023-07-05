using UnityEngine;

namespace Game.Services
{
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