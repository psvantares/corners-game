using System;
using System.Collections.Generic;
using Game.Gameplay.Board;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
using Game.Models;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntry : MonoBehaviour
    {
        [SerializeField]
        private ViewManager viewManager;

        [SerializeField]
        private BoardManager boardManager;

        [SerializeField]
        private ThemeManager themeManager;

        private IGameModel gameModel;

        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            gameModel = new GameModel();

            var gameplayController = new GameplayController(gameModel, viewManager, boardManager, themeManager);

            disposables.Add(gameplayController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }
    }
}