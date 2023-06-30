using System;
using Game.Data;
using Game.Gameplay.Board;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
using Game.Models;
using UniRx;

namespace Game.Gameplay
{
    public class GameplayController : IDisposable
    {
        private readonly ViewManager viewManager;
        private readonly BoardManager boardManager;
        private readonly ThemeManager themeManager;
        private readonly IGameModel gameModel;

        private readonly CompositeDisposable disposable = new();

        public GameplayController(IGameModel gameModel, ViewManager viewManager, BoardManager boardManager, ThemeManager themeManager)
        {
            this.gameModel = gameModel;
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
            viewManager.Initialize(gameModel);
            boardManager.Initialize(gameModel);

            viewManager.StartGameEvent.Subscribe(OnStartGame).AddTo(disposable);
            viewManager.HomeEvent.Subscribe(OnHome).AddTo(disposable);
        }

        // Events

        private void OnStartGame(Unit unit)
        {
            boardManager.StartGame(BoardMode.Normal, true);
        }

        private void OnHome(Unit unit)
        {
            boardManager.Clear();
            viewManager.ShowHome();
        }
    }
}