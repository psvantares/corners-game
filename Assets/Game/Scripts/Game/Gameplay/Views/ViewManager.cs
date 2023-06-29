using System;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class ViewManager : MonoBehaviour
    {
        [Header("VIEWS")]
        [SerializeField]
        private NavigationView navigationView;

        [SerializeField]
        private PlayView playView;

        [SerializeField]
        private MarketView marketView;

        [SerializeField]
        private ProfileView profileView;

        [SerializeField]
        private SettingsView settingsView;

        [SerializeField]
        private BoardView boardView;

        public event Action OnStartGame;
        public event Action<GameStateType> OnPlayPauseGame;

        private void OnEnable()
        {
            navigationView.OnNavigationButton += HandleNavigation;
            playView.OnStartGame += HandleStartGame;
            boardView.OnPlayPause += HandlePlayPause;
        }

        private void OnDisable()
        {
            navigationView.OnNavigationButton -= HandleNavigation;
            playView.OnStartGame -= HandleStartGame;
        }

        private void HandleNavigation(NavigationType navigationType)
        {
            playView.SetActive(navigationType);
            marketView.SetActive(navigationType);
            profileView.SetActive(navigationType);
            settingsView.SetActive(navigationType);
        }

        private void HandleStartGame()
        {
            playView.SetActive(false);
            marketView.SetActive(false);
            profileView.SetActive(false);
            settingsView.SetActive(false);
            navigationView.SetActive(false);

            boardView.SetActive(true);
            boardView.SetActiveTimer(true);

            OnStartGame?.Invoke();
        }

        private void HandlePlayPause(GameStateType type)
        {
            OnPlayPauseGame?.Invoke(type);
        }
    }
}