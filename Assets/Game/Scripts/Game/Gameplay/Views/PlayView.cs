using System;
using Game.Core;
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
        private readonly ISubject<PlayDeckType> deckEvent = new Subject<PlayDeckType>();
        private readonly ISubject<Unit> startGameEvent = new Subject<Unit>();

        public IObservable<PlayDeckType> DeckEvent => deckEvent;
        public IObservable<Unit> StartGameEvent => startGameEvent;

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

        private void OnDeck(PlayDeckType playDeckType)
        {
            deckDiagonalView.SetDeckActive(playDeckType);
            deckSquareView.SetDeckActive(playDeckType);

            deckEvent?.OnNext(playDeckType);
        }

        private void OnStartGame(Unit unit)
        {
            startGameEvent?.OnNext(unit);
        }
    }
}