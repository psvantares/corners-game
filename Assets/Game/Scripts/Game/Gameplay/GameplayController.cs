using System;
using Game.Core;
using Game.Data;
using Game.Services;
using UniRx;
using Object = UnityEngine.Object;


namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly GameplayViewManager viewManager;
        private readonly BoardManager boardManager;
        private readonly BoardAssets assets;
        private readonly PoolController pool;

        private readonly CompositeDisposable disposables = new();
        private readonly CompositeDisposable boardDisposables = new();

        private BoardController boardController;

        public GameplayController
        (
            GameplayViewManager viewManager,
            BoardManager boardManager,
            BoardAssets assets,
            PoolController pool
        )
        {
            this.viewManager = viewManager;
            this.boardManager = boardManager;
            this.assets = assets;
            this.pool = pool;

            Initialize();
        }

        public void Dispose()
        {
            disposables.Clear();
            disposables.Dispose();

            Clear();
        }

        private void Initialize()
        {
            var gameModel = ServiceLocator.Instance.GetService<GameService>().GameModel;
            var config = gameModel.DeckType == BoardDeckType.Diagonal ? assets.BoardConfigDiagonal : assets.BoardConfigSquare;
            var networkGameController = Object.FindObjectOfType<NetworkGameController>();
            var boardContext = new BoardContext(gameModel, networkGameController, config, pool);

            boardController = new BoardController(boardContext);
            boardController.PlayerWinEvent.Subscribe(OnComplete).AddTo(boardDisposables);
            boardManager.StartGame(boardContext, networkGameController.PlayerCount > 0);
        }

        private void Clear()
        {
            boardDisposables.Clear();
            boardController?.Dispose();
            boardController = null;
        }

        // Events

        private void OnComplete(PlayerType playerType)
        {
            Clear();

            pool.Clear();
            boardManager.Clear();
            viewManager.ShowComplete(playerType);
        }
    }
}