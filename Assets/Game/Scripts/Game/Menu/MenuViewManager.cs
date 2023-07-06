using System;
using Game.Data;
using Game.Models;
using UniRx;
using UnityEngine;

namespace Game.Menu
{
    public class MenuViewManager : MonoBehaviour
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

        private IGameModel gameModel;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();
        private readonly ISubject<Unit> homeEvent = new Subject<Unit>();

        public IObservable<Unit> StartGameEvent => startGameEvent;
        public IObservable<Unit> HomeEvent => homeEvent;

        public PlayView PlayView => playView;

        private void OnEnable()
        {
            disposable.Clear();

            navigationView.NavigationEvent.Subscribe(OnNavigation).AddTo(disposable);
            playView.JoinGameEvent.Subscribe(OnJoinGame).AddTo(disposable);
            playView.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
            playView.GameplayModeEvent.Subscribe(OnGameplayMode).AddTo(disposable);
            playView.DeckEvent.Subscribe(OnDeck).AddTo(disposable);
            playView.BoarModeEvent.Subscribe(OnBoardMode).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void Initialize(IGameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void ShowHome()
        {
            navigationView.SetActive(true);

            OnNavigation(NavigationType.Play);
        }

        // Events

        private void OnNavigation(NavigationType navigationType)
        {
            playView.SetActive(navigationType);
            marketView.SetActive(navigationType);
            profileView.SetActive(navigationType);
            settingsView.SetActive(navigationType);
        }

        private void OnJoinGame(Unit unit)
        {
        }

        private void OnStartGame(Unit unit)
        {
            playView.SetActive(false);
            marketView.SetActive(false);
            profileView.SetActive(false);
            settingsView.SetActive(false);
            navigationView.SetActive(false);

            startGameEvent?.OnNext(unit);
        }

        private void OnDeck(BoardDeckType deckType)
        {
            gameModel.DeckType = deckType;
        }

        private void OnGameplayMode(GameplayMode gameplayMode)
        {
            gameModel.GameplayMode = gameplayMode;
        }

        private void OnBoardMode(BoardMode boardMode)
        {
            gameModel.BoardMode = boardMode;
        }
    }
}