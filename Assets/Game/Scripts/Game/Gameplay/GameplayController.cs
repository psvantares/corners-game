using System;
using Game.Data;
using Game.Gameplay.Board;
using Game.Gameplay.Pool;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
using Game.Models;
using UniRx;

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
        private readonly PoolController pool;

        private readonly CompositeDisposable disposable = new();

        private BoardController boardController;

        public GameplayController
        (
            IGameModel gameModel,
            ViewManager viewManager,
            BoardManager boardManager,
            ThemeManager themeManager,
            BoardAssets assets,
            BoardProvider provider,
            PoolController pool
        )
        {
            this.gameModel = gameModel;
            this.viewManager = viewManager;
            this.boardManager = boardManager;
            this.themeManager = themeManager;
            this.assets = assets;
            this.provider = provider;
            this.pool = pool;

            Initialize();
        }

        public void Dispose()
        {
            disposable.Clear();
            disposable.Dispose();
        }

        private void Initialize()
        {
            viewManager.Initialize(gameModel);
            viewManager.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
            viewManager.HomeEvent.Subscribe(OnHome).AddTo(disposable);
        }

        private void Clear()
        {
            boardController?.Dispose();
            boardController = null;
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            var config = gameModel.DeckType == BoardDeckType.Diagonal ? assets.BoardConfigDiagonal : assets.BoardConfigSquare;
            var boardContext = new BoardContext(gameModel, config, pool);

            boardController = new BoardController(boardContext);
            boardManager.StartGame(boardContext);
        }

        private void OnHome(Unit unit)
        {
            Clear();

            pool.Clear();
            boardManager.Clear();
            viewManager.ShowHome();
        }
    }
}