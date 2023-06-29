using System;
using Game.Core;
using Game.Gameplay.Theme;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Views
{
    public enum PlayDeckType
    {
        Square,
        Diagonal
    }

    public class PlayDeckTypeView : ViewBase
    {
        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton playDeckButton;

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

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<PlayDeckType> clickEvent = new Subject<PlayDeckType>();

        public IObservable<PlayDeckType> ClickEvent => clickEvent;

        private void OnEnable()
        {
            disposable.Clear();

            playDeckButton.ClickedEvent.Subscribe(_ => OnClick(playDeckType)).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void SetDeckActive(PlayDeckType select)
        {
            var active = playDeckType == select;
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

        // Events

        private void OnClick(PlayDeckType playDeckType)
        {
            clickEvent?.OnNext(playDeckType);
        }
    }
}