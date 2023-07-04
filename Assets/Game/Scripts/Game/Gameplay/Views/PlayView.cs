using System;
using Game.Core;
using Game.Data;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class PlayView : ViewBase
    {
        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton startGameButton;

        [Header("TOGGLES")]
        [SerializeField]
        private SmartToggle botToggle;

        [SerializeField]
        private SmartToggle playerToggle;

        [SerializeField]
        private SmartToggle networkToggle;

        [Space]
        [SerializeField]
        private SmartToggle diagonalToggle;

        [SerializeField]
        private SmartToggle squareToggle;

        [Space]
        [SerializeField]
        private SmartToggle boardNormalToggle;

        [SerializeField]
        private SmartToggle boardDiagonalToggle;

        [SerializeField]
        private SmartToggle boardVerticalHorizontalToggle;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<BoardDeckType> deckEvent = new Subject<BoardDeckType>();
        private readonly ISubject<GameplayMode> gameplayModeEvent = new Subject<GameplayMode>();
        private readonly ISubject<BoardMode> boarModeEvent = new Subject<BoardMode>();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();

        public IObservable<BoardDeckType> DeckEvent => deckEvent;
        public IObservable<GameplayMode> GameplayModeEvent => gameplayModeEvent;
        public IObservable<BoardMode> BoarModeEvent => boarModeEvent;
        public IObservable<Unit> StartGameEvent => startGameEvent;

        public void OnEnable()
        {
            disposable.Clear();

            startGameButton.ClickedEvent.Subscribe(OnStartGame).AddTo(disposable);

            botToggle.ClickedEvent.Subscribe(_ => OnGameplayMode(GameplayMode.Bot)).AddTo(disposable);
            playerToggle.ClickedEvent.Subscribe(_ => OnGameplayMode(GameplayMode.Player)).AddTo(disposable);
            networkToggle.ClickedEvent.Subscribe(_ => OnGameplayMode(GameplayMode.Network)).AddTo(disposable);

            diagonalToggle.ClickedEvent.Subscribe(_ => OnDeck(BoardDeckType.Diagonal)).AddTo(disposable);
            squareToggle.ClickedEvent.Subscribe(_ => OnDeck(BoardDeckType.Square)).AddTo(disposable);

            boardNormalToggle.ClickedEvent.Subscribe(_ => OnBoardMode(BoardMode.Normal)).AddTo(disposable);
            boardDiagonalToggle.ClickedEvent.Subscribe(_ => OnBoardMode(BoardMode.Diagonal)).AddTo(disposable);
            boardVerticalHorizontalToggle.ClickedEvent.Subscribe(_ => OnBoardMode(BoardMode.VerticalHorizontal)).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        // Events

        private void OnDeck(BoardDeckType deckType)
        {
            deckEvent?.OnNext(deckType);
        }

        private void OnGameplayMode(GameplayMode gameplayMode)
        {
            gameplayModeEvent?.OnNext(gameplayMode);
        }

        private void OnBoardMode(BoardMode boardMode)
        {
            boarModeEvent?.OnNext(boardMode);
        }

        private void OnStartGame(Unit unit)
        {
            startGameEvent?.OnNext(unit);
        }
    }
}