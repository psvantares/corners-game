using System;
using Game.Core;
using Game.Data;
using Game.Services;
using UniRx;

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
        }

        private void Initialize()
        {
            var gameModel = ServiceLocator.Instance.GetService<GameService>().GameModel;
            var config = gameModel.DeckType == BoardDeckType.Diagonal ? assets.BoardConfigDiagonal : assets.BoardConfigSquare;
            var boardContext = new BoardContext(gameModel, config, pool);

            boardController = new BoardController(boardContext);
            boardController.PlayerWinEvent.Subscribe(OnPlayerWin).AddTo(boardDisposables);
            boardManager.StartGame(boardContext);
        }

        private void Clear()
        {
            boardDisposables.Clear();
            boardController?.Dispose();
            boardController = null;
        }

        // Events

        private void OnPlayerWin(PlayerType playerType)
        {
            Clear();

            pool.Clear();
            boardManager.Clear();
            viewManager.ShowComplete(playerType);
        }
    }
}