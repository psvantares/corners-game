using System;
using Fusion;
using Game.Data;
using Game.Gameplay.Board;
using Game.Gameplay.Pool;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
using Game.Models;
using Game.Network;
using UniRx;
using Object = UnityEngine.Object;

namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly IGameModel gameModel;
        private readonly ViewManager viewManager;
        private readonly BoardManager boardManager;
        private readonly ThemeManager themeManager;
        private readonly BoardAssets assets;
        private readonly BoardProvider provider;
        private readonly NetworkRunner networkRunnerPrefab;
        private readonly PoolController pool;

        private readonly CompositeDisposable disposables = new();
        private readonly CompositeDisposable boardDisposables = new();

        private BoardController boardController;
        private NetworkRunner runnerInstance;

        public GameplayController
        (
            IGameModel gameModel,
            ViewManager viewManager,
            BoardManager boardManager,
            ThemeManager themeManager,
            BoardAssets assets,
            BoardProvider provider,
            NetworkRunner networkRunnerPrefab,
            PoolController pool
        )
        {
            this.gameModel = gameModel;
            this.viewManager = viewManager;
            this.boardManager = boardManager;
            this.themeManager = themeManager;
            this.assets = assets;
            this.provider = provider;
            this.networkRunnerPrefab = networkRunnerPrefab;
            this.pool = pool;

            Initialize();
        }

        public void Dispose()
        {
            disposables.Clear();
            disposables.Dispose();
        }

        private void Initialize()
        {
            viewManager.Initialize(gameModel);
            viewManager.StartGameEvent.Subscribe(OnStartGame).AddTo(disposables);
            viewManager.HomeEvent.Subscribe(OnHome).AddTo(disposables);
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
                ObjectPool = runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
            };

            await runnerInstance.StartGame(startGameArgs);

            // runnerInstance.SetActiveScene(sceneName);
        }

        private void Clear()
        {
            boardDisposables.Clear();
            boardController?.Dispose();
            boardController = null;
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            var config = gameModel.DeckType == BoardDeckType.Diagonal ? assets.BoardConfigDiagonal : assets.BoardConfigSquare;
            var boardContext = new BoardContext(gameModel, config, pool);

            boardController = new BoardController(boardContext);
            boardController.PlayerWinEvent.Subscribe(OnPlayerWin).AddTo(boardDisposables);
            boardManager.StartGame(boardContext);
        }

        private void OnHome(Unit unit)
        {
            Clear();

            pool.Clear();
            boardManager.Clear();
            viewManager.ShowHome();
        }

        private void OnPlayerWin(PlayerType playerType)
        {
            Clear();

            pool.Clear();
            boardManager.Clear();
            viewManager.ShowWin(playerType);
        }
    }
}