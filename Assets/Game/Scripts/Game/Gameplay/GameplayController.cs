using System;
using Game.Core;
using Game.Data;
using Game.Models;
using Game.Services;
using UniRx;

namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly IGameModel gameModel;
        private readonly BoardManager boardManager;
        private readonly BoardAssets assets;
        private readonly BoardProvider provider;
        private readonly PoolController pool;

        private readonly CompositeDisposable disposables = new();
        private readonly CompositeDisposable boardDisposables = new();

        private BoardController boardController;

        public GameplayController
        (
            BoardManager boardManager,
            BoardAssets assets,
            BoardProvider provider,
            PoolController pool
        )
        {
            this.boardManager = boardManager;
            this.assets = assets;
            this.provider = provider;
            this.pool = pool;

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
            // viewManager.ShowWin(playerType);
        }
    }
}