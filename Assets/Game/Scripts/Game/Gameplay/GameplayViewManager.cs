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

        private NetworkRunner networkRunner;
        private readonly CompositeDisposable disposable = new();

        private void Start()
        {
            networkRunner = FindObjectOfType<NetworkRunner>();
        }

        private void OnEnable()
        {
            disposable.Clear();

            gameplayView.MenuEvent.Subscribe(OnShowMenu).AddTo(disposable);
            completeView.ContinueEvent.Subscribe(OnShowMenu).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void ShowComplete(PlayerType playerType)
        {
            completeView.SetTitleText(playerType);
            completeView.SetActive(true);
        }

        // Events

        private void OnShowMenu(Unit unit)
        {
            networkRunner.Shutdown();
        }
    }
}