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

        [SerializeField]
        private WinView winView;

        private IGameModel gameModel;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();
        private readonly ISubject<Unit> homeEvent = new Subject<Unit>();

        public IObservable<Unit> StartGameEvent => startGameEvent;
        public IObservable<Unit> HomeEvent => homeEvent;

        private void OnEnable()
        {
            disposable.Clear();

            navigationView.NavigationEvent.Subscribe(OnNavigation).AddTo(disposable);
            playView.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
            playView.GameplayModeEvent.Subscribe(OnGameplayMode).AddTo(disposable);
            playView.DeckEvent.Subscribe(OnDeck).AddTo(disposable);
            playView.BoarModeEvent.Subscribe(OnBoardMode).AddTo(disposable);
            boardView.HomeEvent.Subscribe(OnHome).AddTo(disposable);
            winView.ContinueEvent.Subscribe(OnContinue).AddTo(disposable);
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

            boardView.SetActive(false);
            boardView.SetActiveTimer(false);

            OnNavigation(NavigationType.Play);
        }

        public void ShowWin(PlayerType playerType)
        {
            boardView.SetActive(false);
            boardView.SetActiveTimer(false);

            winView.SetTitleText(playerType);
            winView.SetActive(true);
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

        private void OnGameplayMode(GameplayMode gameplayMode)
        {
            gameModel.GameplayMode = gameplayMode;
        }

        private void OnBoardMode(BoardMode boardMode)
        {
            gameModel.BoardMode = boardMode;
        }

        private void OnHome(Unit unit)
        {
            homeEvent?.OnNext(unit);
        }

        private void OnContinue(Unit unit)
        {
            navigationView.SetActive(true);
            winView.SetActive(false);

            OnNavigation(NavigationType.Play);
        }
    }
}