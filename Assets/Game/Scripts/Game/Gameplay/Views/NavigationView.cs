using System;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class NavigationView : ViewBase
    {
        [Header("VIEWS")]
        [SerializeField]
        private NavigationButtonView buttonPlayView;

        [SerializeField]
        private NavigationButtonView buttonMarketView;

        [SerializeField]
        private NavigationButtonView buttonProfileView;

        [SerializeField]
        private NavigationButtonView buttonSettingsView;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<NavigationType> navigationEvent = new Subject<NavigationType>();

        public IObservable<NavigationType> NavigationEvent => navigationEvent;

        public void OnEnable()
        {
            disposable.Clear();

            buttonPlayView.ClickEvent.Subscribe(OnNavigation).AddTo(disposable);
            buttonMarketView.ClickEvent.Subscribe(OnNavigation).AddTo(disposable);
            buttonProfileView.ClickEvent.Subscribe(OnNavigation).AddTo(disposable);
            buttonSettingsView.ClickEvent.Subscribe(OnNavigation).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        // Events

        private void OnNavigation(NavigationType type)
        {
            buttonPlayView.SetActive(type);
            buttonMarketView.SetActive(type);
            buttonProfileView.SetActive(type);
            buttonSettingsView.SetActive(type);

            navigationEvent?.OnNext(type);
        }
    }
}