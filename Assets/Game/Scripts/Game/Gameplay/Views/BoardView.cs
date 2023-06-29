using System;
using Game.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class BoardView : ViewBase
    {
        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text timerText;

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton homeButton;

        private float timeRemaining;
        private bool timerIsRunning;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> homeEvent = new Subject<Unit>();

        public IObservable<Unit> HomeEvent => homeEvent;

        private void Update()
        {
            if (!timerIsRunning)
            {
                return;
            }

            DisplayTime(timeRemaining += Time.deltaTime);
        }

        private void OnEnable()
        {
            disposable.Clear();

            homeButton.ClickedEvent.Subscribe(OnHome).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void SetActiveTimer(bool active)
        {
            timerIsRunning = active;
            timeRemaining = 0;
        }

        private void DisplayTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        // Events

        private void OnHome(Unit unit)
        {
            homeEvent?.OnNext(unit);
        }
    }
}