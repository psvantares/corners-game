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

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<BoardDeckType> deckEvent = new Subject<BoardDeckType>();
        private readonly ISubject<GameMode> gameModeEvent = new Subject<GameMode>();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();

        public IObservable<BoardDeckType> DeckEvent => deckEvent;
        public IObservable<GameMode> GameModeEvent => gameModeEvent;
        public IObservable<Unit> StartGameEvent => startGameEvent;

        protected override void Start()
        {
            base.Start();

            ForceDeckType(BoardDeckType.Square);
            ForceGameMode(GameMode.Bot);
        }

        public void OnEnable()
        {
            disposable.Clear();

            startGameButton.ClickedEvent.Subscribe(OnStartGame).AddTo(disposable);

            botToggle.ClickedEvent.Subscribe(_ => OnGameMode(GameMode.Bot)).AddTo(disposable);
            playerToggle.ClickedEvent.Subscribe(_ => OnGameMode(GameMode.Player)).AddTo(disposable);
            networkToggle.ClickedEvent.Subscribe(_ => OnGameMode(GameMode.Network)).AddTo(disposable);

            diagonalToggle.ClickedEvent.Subscribe(_ => OnDeck(BoardDeckType.Diagonal)).AddTo(disposable);
            squareToggle.ClickedEvent.Subscribe(_ => OnDeck(BoardDeckType.Square)).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        private void ForceDeckType(BoardDeckType deckType)
        {
            if (deckType == BoardDeckType.Square)
            {
                squareToggle.ForceClick(true);
            }
            else if (deckType == BoardDeckType.Diagonal)
            {
                diagonalToggle.ForceClick(true);
            }
        }

        private void ForceGameMode(GameMode gameMode)
        {
            if (gameMode == GameMode.Bot)
            {
                botToggle.ForceClick(true);
            }
            else if (gameMode == GameMode.Player)
            {
                playerToggle.ForceClick(true);
            }
            else if (gameMode == GameMode.Network)
            {
                networkToggle.ForceClick(true);
            }
        }

        // Events

        private void OnDeck(BoardDeckType deckType)
        {
            deckEvent?.OnNext(deckType);
        }

        private void OnGameMode(GameMode gameMode)
        {
            gameModeEvent?.OnNext(gameMode);
        }

        private void OnStartGame(Unit unit)
        {
            startGameEvent?.OnNext(unit);
        }
    }
}