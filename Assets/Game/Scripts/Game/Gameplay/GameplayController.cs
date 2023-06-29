using System;
using Game.Gameplay.Board;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
using UniRx;

namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly ViewManager viewManager;
        private readonly BoardManager boardManager;
        private readonly ThemeManager themeManager;

        private readonly CompositeDisposable disposable = new();

        public GameplayController(ViewManager viewManager, BoardManager boardManager, ThemeManager themeManager)
        {
            this.viewManager = viewManager;
            this.boardManager = boardManager;
            this.themeManager = themeManager;

            Initialize();
        }

        public void Dispose()
        {
            disposable.Clear();
            disposable.Dispose();
        }

        private void Initialize()
        {
            viewManager.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            boardManager.StartGame(BoardMode.Normal, true);
        }
    }
}