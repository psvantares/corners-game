using UnityEngine;
using UnityEngine.UI;

namespace Game.Services
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic))]
    public class ThemeGraphic : MonoBehaviour
    {
        [SerializeField]
        protected ThemeColor ThemeColor;

        [SerializeField]
        protected Graphic Graphic;

        private void OnEnable()
        {
            ApplyTheme();

            ThemeManager.OnThemeEvent += ApplyTheme;
        }

        private void OnDisable() => ThemeManager.OnThemeEvent -= ApplyTheme;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Graphic == null)
            {
                Graphic = GetComponent<Graphic>();
            }

            if (Graphic != null)
            {
                Graphic.color = ThemeManager.GetColor(ThemeColor);
            }
        }
#endif

        private void ApplyTheme() => Graphic.color = ThemeManager.GetColor(ThemeColor);
    }
}