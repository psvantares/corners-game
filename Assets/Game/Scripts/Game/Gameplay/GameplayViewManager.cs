using Fusion;
using Game.Data;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayViewManager : MonoBehaviour
    {
        [Header("VIEWS")]
        [SerializeField]
        private PlayerView playView;

        [SerializeField]
        private GameplayView gameplayView;

        [SerializeField]
        private CompleteView completeView;

        private bool isInitialize;

        private NetworkRunner networkRunner;
        private NetworkGameController networkGameController;
        private readonly CompositeDisposable disposable = new();

        private void OnDestroy()
        {
            disposable.Clear();
        }

        public void Initialize()
        {
            if (isInitialize)
            {
                return;
            }

            networkRunner = FindObjectOfType<NetworkRunner>();
            networkGameController = FindObjectOfType<NetworkGameController>();

            Subscribes();

            isInitialize = true;
        }

        public void ShowComplete(PlayerType playerType)
        {
            completeView.SetTitleText(playerType);
            completeView.SetActive(true);
        }

        private void Subscribes()
        {
            disposable.Clear();

            networkGameController.DisconnectEvent.Subscribe(OnDisconnect).AddTo(disposable);
            networkGameController.RemainingEvent.Subscribe(OnRemaining).AddTo(disposable);
            networkGameController.SwitchPlayerEvent.Subscribe(OnRemaining).AddTo(disposable);

            gameplayView.MenuEvent.Subscribe(OnShowMenu).AddTo(disposable);
            completeView.ContinueEvent.Subscribe(OnShowMenu).AddTo(disposable);
        }

        // Events

        private void OnShowMenu(Unit unit)
        {
            if (!isInitialize)
            {
                return;
            }

            networkRunner.Shutdown();
        }

        private void OnDisconnect(string text)
        {
            gameplayView.DisconnectText(text);
        }

        private void OnRemaining(string text)
        {
            gameplayView.RemainingText(text);
        }

        private void OnRemaining(PlayerType playerType)
        {
            gameplayView.PlayerText(playerType.ToString());
        }
    }
}