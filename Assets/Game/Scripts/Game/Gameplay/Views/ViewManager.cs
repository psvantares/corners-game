using System;
using Game.Data;
using Game.Models;
using UniRx;
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

        private IGameModel gameModel;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();
        private readonly ISubject<GameStateType> playPauseGameEvent = new Subject<GameStateType>();

        public IObservable<Unit> StartGameEvent => startGameEvent;
        public IObservable<GameStateType> PlayPauseGameEvent => playPauseGameEvent;

        private void OnEnable()
        {
            disposable.Clear();

            navigationView.NavigationEvent.Subscribe(OnNavigation).AddTo(disposable);
            playView.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
            playView.DeckEvent.Subscribe(OnDeck).AddTo(disposable);
            boardView.PlayPauseGameEvent.Subscribe(OnPlayPauseGame).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void Initialize(IGameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        // Events

        private void OnNavigation(NavigationType navigationType)
        {
            playView.SetActive(navigationType);
            marketView.SetActive(navigationType);
            profileView.SetActive(navigationType);
            settingsView.SetActive(navigationType);
        }

        private void OnStartGame(Unit unit)
        {
            playView.SetActive(false);
            marketView.SetActive(false);
            profileView.SetActive(false);
            settingsView.SetActive(false);
            navigationView.SetActive(false);

            boardView.SetActive(true);
            boardView.SetActiveTimer(true);

            startGameEvent?.OnNext(unit);
        }

        private void OnDeck(BoardDeckType deckType)
        {
            gameModel.DeckType = deckType;
        }

        private void OnPlayPauseGame(GameStateType type)
        {
            playPauseGameEvent?.OnNext(type);
        }
    }
}