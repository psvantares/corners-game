using System;
using Game.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public enum GameStateType
    {
        Play,
        Pause
    }

    public class BoardView : ViewBase
    {
        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text timerText;

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton playPauseButton;

        [Header("SETTINGS")]
        [SerializeField]
        private float timeRemaining = 10;

        private bool timerIsRunning;
        private GameStateType currentState = GameStateType.Play;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<GameStateType> playPauseGameEvent = new Subject<GameStateType>();

        public IObservable<GameStateType> PlayPauseGameEvent => playPauseGameEvent;

        private void Update()
        {
            if (!timerIsRunning)
            {
                return;
            }

            if (currentState == GameStateType.Play)
            {
                DisplayTime(timeRemaining += Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            disposable.Clear();

            playPauseButton.ClickedEvent.Subscribe(OnPlayPauseGame).AddTo(disposable);
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
        private void OnPlayPauseGame(Unit unit)
        {
            currentState = currentState switch
            {
                GameStateType.Play => GameStateType.Pause,
                GameStateType.Pause => GameStateType.Play,
                _ => currentState
            };

            playPauseGameEvent?.OnNext(currentState);
        }
    }
}