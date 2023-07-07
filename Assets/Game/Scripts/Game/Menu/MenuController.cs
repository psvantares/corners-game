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
        private readonly LoaderService loaderService;
        private readonly MenuViewManager menuViewManager;
        private readonly PlayerData playerDataPrefab;
        private readonly NetworkRunner networkRunnerPrefab;

        private readonly CompositeDisposable disposables = new();

        private NetworkRunner runnerInstance;

        private string roomName;

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
            loaderService = ServiceLocator.Instance.GetService<LoaderService>();

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

            if (!string.IsNullOrEmpty(menuViewManager.PlayView.NickName))
            {
                playerData.SetNickName(menuViewManager.PlayView.NickName);
            }

            roomName = !string.IsNullOrEmpty(menuViewManager.PlayView.RoomName) ? menuViewManager.PlayView.RoomName : "DEFAULT";
        }

        private async void StartGame(GameMode mode, string roomName, string sceneName)
        {
            loaderService.Show();

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
                PlayerCount = 2,
                SceneManager = runnerInstance.gameObject.AddComponent<NetworkSceneManagerDefault>()
            };

            await runnerInstance.StartGame(startGameArgs);

            runnerInstance.SetActiveScene(sceneName);

            loaderService.Hide();
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            StartPlayerData();
            StartGame(gameModel.GameplayMode == GameplayMode.Network ? GameMode.Shared : GameMode.Single, roomName, "Gameplay");
        }

        private void OnHome(Unit unit)
        {
            runnerInstance.Shutdown();
            menuViewManager.ShowHome();
        }
    }
}