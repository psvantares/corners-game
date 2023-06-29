using System;
using Game.Gameplay.Theme;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Views
{
    public enum NavigationType
    {
        None,
        Play,
        Market,
        Profile,
        Settings
    }

    public class NavigationButtonView : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button navigationButton;

        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text navigationText;

        [Header("IMAGES")]
        [SerializeField]
        private Image navigationIcon;

        [Header("SETTINGS")]
        [SerializeField]
        private NavigationType navigationType;

        public event Action<NavigationType> OnClick;

        public NavigationType NavigationType => navigationType;

        private void OnEnable()
        {
            navigationButton.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            navigationButton.onClick.RemoveAllListeners();
        }

        public void SetActive(bool active)
        {
            var activeColor = ThemeManager.GetColor(ThemeColor.ColorB);
            var disableColor = ThemeManager.GetColor(ThemeColor.ColorD);

            navigationText.color = active ? activeColor : disableColor;
            navigationIcon.color = active ? activeColor : disableColor;
        }

        private void HandleClick()
        {
            OnClick?.Invoke(navigationType);
        }
    }
}