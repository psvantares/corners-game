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

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton homeButton;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> homeEvent = new Subject<Unit>();

        public IObservable<Unit> HomeEvent => homeEvent;

        private void OnEnable()
        {
            disposable.Clear();

            homeButton.ClickedEvent.Subscribe(OnHome).AddTo(disposable);
            EventBus.RemainingTimeEvent.Subscribe(DisplayTime).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        private void DisplayTime(float tick)
        {
            var time = TimeSpan.FromSeconds(tick);
            timerText.text = time.ToString(@"hh\:mm\:ss");
        }

        // Events

        private void OnHome(Unit unit)
        {
            homeEvent?.OnNext(unit);
        }
    }
}