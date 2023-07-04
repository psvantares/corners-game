using System;
using System.Collections.Generic;
using Fusion;
using Game.Gameplay.Board;
using Game.Gameplay.Pool;
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

        [Space]
        [SerializeField]
        private BoardAssets boardAssets;

        [SerializeField]
        private BoardProvider boardProvider;

        [Space]
        [SerializeField]
        private NetworkRunner networkRunnerPrefab;

        private IGameModel gameModel;

        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            gameModel = new GameModel();

            var poolController = new PoolController(boardAssets, boardProvider);
            var gameplayController = new GameplayController
            (
                gameModel,
                viewManager,
                boardManager,
                themeManager,
                boardAssets,
                boardProvider,
                networkRunnerPrefab,
                poolController
            );

            disposables.Add(gameplayController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }
    }
}