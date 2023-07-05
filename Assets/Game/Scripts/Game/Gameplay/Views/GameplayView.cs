using System;
using Game.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayView : ViewBase
    {
        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text timerText;

        [SerializeField]
        private TMP_Text disconnectText;

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton homeButton;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> menuEvent = new Subject<Unit>();

        public IObservable<Unit> MenuEvent => menuEvent;

        private void OnEnable()
        {
            disposable.Clear();

            homeButton.ClickedEvent.Subscribe(OnMenu).AddTo(disposable);
            EventBus.RemainingTimeEvent.Subscribe(OnRemaining).AddTo(disposable);
            EventBus.GameStateEndingEvent.Subscribe(OnDisconnect).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        // Events

        private void OnMenu(Unit unit)
        {
            menuEvent?.OnNext(unit);
        }

        private void OnRemaining(string time)
        {
            timerText.text = time;
        }

        private void OnDisconnect(string text)
        {
            disconnectText.text = text;
        }
    }
}