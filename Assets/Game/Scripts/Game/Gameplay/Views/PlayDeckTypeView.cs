using System;
using Game.Gameplay.Theme;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Views
{
    public enum PlayDeckType
    {
        Square,
        Diagonal
    }

    public class PlayDeckTypeView : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button playDeckButton;

        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text playDeckText;

        [Header("IMAGES")]
        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image[] dotImage;

        [Header("GAME OBJECTS")]
        [SerializeField]
        private GameObject markGo;

        [Header("SETTINGS")]
        [SerializeField]
        private PlayDeckType playDeckType;

        public event Action<PlayDeckType> OnClick;

        public PlayDeckType PlayDeckType => playDeckType;

        private void OnEnable()
        {
            playDeckButton.onClick.AddListener(HandleClick);
        }

        private void OnDisable()
        {
            playDeckButton.onClick.RemoveAllListeners();
        }

        public void SetActive(bool active)
        {
            var activeColor = ThemeManager.GetColor(ThemeColor.ColorB);
            var disableColor = ThemeManager.GetColor(ThemeColor.ColorG);
            var backgroundActiveColor = ThemeManager.GetColor(ThemeColor.ColorE);
            var backgroundDisableColor = ThemeManager.GetColor(ThemeColor.ColorG);
            var dotActiveColor = ThemeManager.GetColor(ThemeColor.ColorA);
            var dotDisableColor = ThemeManager.GetColor(ThemeColor.ColorF);

            playDeckText.color = active ? activeColor : disableColor;
            backgroundImage.color = active ? backgroundActiveColor : backgroundDisableColor;

            foreach (var dot in dotImage)
            {
                dot.color = active ? dotActiveColor : dotDisableColor;
            }

            markGo.SetActive(active);
        }

        private void HandleClick()
        {
            OnClick?.Invoke(playDeckType);
        }
    }
}