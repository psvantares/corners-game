using System;
using Game.Core;
using Game.Data;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class PlayView : ViewBase
    {
        [Header("VIEWS")]
        [SerializeField]
        private PlayDeckTypeView deckDiagonalView;

        [SerializeField]
        private PlayDeckTypeView deckSquareView;

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton startGameButton;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<BoardDeckType> deckEvent = new Subject<BoardDeckType>();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();

        public IObservable<BoardDeckType> DeckEvent => deckEvent;
        public IObservable<Unit> StartGameEvent => startGameEvent;

        protected override void Start()
        {
            base.Start();

            OnDeck(BoardDeckType.Square);
        }

        public void OnEnable()
        {
            disposable.Clear();

            deckDiagonalView.ClickEvent.Subscribe(OnDeck).AddTo(disposable);
            deckSquareView.ClickEvent.Subscribe(OnDeck).AddTo(disposable);
            startGameButton.ClickedEvent.Subscribe(OnStartGame).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        // Events

        private void OnDeck(BoardDeckType deckType)
        {
            deckDiagonalView.SetDeckActive(deckType);
            deckSquareView.SetDeckActive(deckType);

            deckEvent?.OnNext(deckType);
        }

        private void OnStartGame(Unit unit)
        {
            startGameEvent?.OnNext(unit);
        }
    }
}