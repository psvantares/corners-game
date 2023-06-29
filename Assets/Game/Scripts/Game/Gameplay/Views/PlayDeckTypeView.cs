using System;
using Game.Core;
using Game.Data;
using Game.Gameplay.Theme;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Views
{
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
        private BoardDeckType deckType;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<BoardDeckType> clickEvent = new Subject<BoardDeckType>();

        public IObservable<BoardDeckType> ClickEvent => clickEvent;

        private void OnEnable()
        {
            disposable.Clear();

            playDeckButton.ClickedEvent.Subscribe(_ => OnClick(deckType)).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void SetDeckActive(BoardDeckType select)
        {
            var active = deckType == select;
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

        private void OnClick(BoardDeckType deckType)
        {
            clickEvent?.OnNext(deckType);
        }
    }
}