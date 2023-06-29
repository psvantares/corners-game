using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Gameplay.Views
{
    public class PlayView : ViewBase
    {
        [Header("VIEWS")]
        [SerializeField]
        private PlayDeckTypeView[] deckView;

        [Header("BUTTONS")]
        [SerializeField]
        private Button startGameButton;

        public event Action<PlayDeckType> OnDeck;
        public event Action OnStartGame;

        public void OnEnable()
        {
            foreach (var view in deckView)
            {
                view.OnClick += HandleDeck;
            }

            startGameButton.onClick.AddListener(HandleStartGame);
        }

        private void OnDisable()
        {
            foreach (var view in deckView)
            {
                view.OnClick -= HandleDeck;
            }

            startGameButton.onClick.RemoveAllListeners();
        }

        private void HandleDeck(PlayDeckType type)
        {
            foreach (var view in deckView)
            {
                view.SetActive(type == view.PlayDeckType);
            }

            OnDeck?.Invoke(type);
        }

        private void HandleStartGame()
        {
            OnStartGame?.Invoke();
        }
    }
}