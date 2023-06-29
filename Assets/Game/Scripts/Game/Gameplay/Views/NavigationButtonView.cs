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
    public class NavigationButtonView : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton navigationButton;

        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text navigationText;

        [Header("IMAGES")]
        [SerializeField]
        private Image navigationIcon;

        [Header("SETTINGS")]
        [SerializeField]
        private NavigationType navigationType;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<NavigationType> clickEvent = new Subject<NavigationType>();

        public IObservable<NavigationType> ClickEvent => clickEvent;

        private void OnEnable()
        {
            disposable.Clear();

            navigationButton.ClickedEvent.Subscribe(OnClick).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void SetActive(NavigationType select)
        {
            var active = navigationType == select;
            var activeColor = ThemeManager.GetColor(ThemeColor.ColorB);
            var disableColor = ThemeManager.GetColor(ThemeColor.ColorD);

            navigationText.color = active ? activeColor : disableColor;
            navigationIcon.color = active ? activeColor : disableColor;
        }

        // Events

        private void OnClick(Unit unit)
        {
            clickEvent?.OnNext(navigationType);
        }
    }
}