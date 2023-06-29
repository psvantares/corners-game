using System;
using Game.Gameplay.Board;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;

namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly ViewManager viewManager;
        private readonly BoardManager boardManager;
        private readonly ThemeManager themeManager;

        public GameplayController(ViewManager viewManager, BoardManager boardManager, ThemeManager themeManager)
        {
            this.viewManager = viewManager;
            this.boardManager = boardManager;
            this.themeManager = themeManager;

            Initialize();
        }

        public void Dispose()
        {
            viewManager.OnStartGame -= HandleStartGame;
        }

        private void Initialize()
        {
            viewManager.OnStartGame += HandleStartGame;
        }

        private void HandleStartGame()
        {
            boardManager.StartGame(BoardMode.Normal, true);
        }
    }
}