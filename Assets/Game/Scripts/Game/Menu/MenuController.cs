using System;
using Fusion;
using Game.Core;
using Game.Data;
using Game.Models;
using Game.Services;
using UniRx;
using Object = UnityEngine.Object;

namespace Game.Menu
{
    public class MenuController : IDisposable
    {
        private readonly IGameModel gameModel;
        private readonly MenuViewManager menuViewManager;
        private readonly PlayerData playerDataPrefab;
        private readonly NetworkRunner networkRunnerPrefab;

        private readonly CompositeDisposable disposables = new();

        private NetworkRunner runnerInstance;

        public MenuController
        (
            MenuViewManager menuViewManager,
            PlayerData playerDataPrefab,
            NetworkRunner networkRunnerPrefab
        )
        {
            this.menuViewManager = menuViewManager;
            this.playerDataPrefab = playerDataPrefab;
            this.networkRunnerPrefab = networkRunnerPrefab;

            gameModel = ServiceLocator.Instance.GetService<GameService>().GameModel;

            Initialize();
        }

        public void Dispose()
        {
            disposables.Clear();
            disposables.Dispose();
        }

        private void Initialize()
        {
            menuViewManager.Initialize(gameModel);
            menuViewManager.StartGameEvent.Subscribe(OnStartGame).AddTo(disposables);
            menuViewManager.HomeEvent.Subscribe(OnHome).AddTo(disposables);
        }

        private void StartPlayerData()
        {
            var playerData = Object.FindObjectOfType<PlayerData>();

            if (playerData == null)
            {
                playerData = Object.Instantiate(playerDataPrefab);
            }

            playerData.SetNickName("neo");
        }

        private async void StartGame(GameMode mode, string roomName, string sceneName)
        {
            runnerInstance = Object.FindObjectOfType<NetworkRunner>();

            if (runnerInstance == null)
            {
                runnerInstance = Object.Instantiate(networkRunnerPrefab);
            }

            runnerInstance.ProvideInput = true;

            var startGameArgs = new StartGameArgs
            {
                GameMode = mode,
                SessionName = roomName,
                Scene = 2,
                SceneManager = runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>()
            };

            await runnerInstance.StartGame(startGameArgs);

            runnerInstance.SetActiveScene(sceneName);
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            StartPlayerData();
            StartGame(gameModel.GameplayMode == GameplayMode.Network ? GameMode.Single : GameMode.Shared, "default", "Gameplay");
        }

        private void OnHome(Unit unit)
        {
            runnerInstance.Shutdown();
            menuViewManager.ShowHome();
        }
    }
}