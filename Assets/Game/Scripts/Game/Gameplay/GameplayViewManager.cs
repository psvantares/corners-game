using Fusion;
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

            gameplayView.HomeEvent.Subscribe(OnShowMenu).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        // Events

        private void OnShowMenu(Unit unit)
        {
            networkRunner.Shutdown();
        }
    }
}