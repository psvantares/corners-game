using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private Button playPauseButton;

        [Header("SETTINGS")]
        [SerializeField]
        private float timeRemaining = 10;

        private bool timerIsRunning;

        private GameStateType currentState = GameStateType.Play;

        public event Action<GameStateType> OnPlayPause;

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
            playPauseButton.onClick.AddListener(HandlePlayPause);
        }

        private void OnDisable()
        {
            playPauseButton.onClick.RemoveAllListeners();
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

        private void HandlePlayPause()
        {
            currentState = currentState switch
            {
                GameStateType.Play => GameStateType.Pause,
                GameStateType.Pause => GameStateType.Play,
                _ => currentState
            };

            OnPlayPause?.Invoke(currentState);
        }
    }
}