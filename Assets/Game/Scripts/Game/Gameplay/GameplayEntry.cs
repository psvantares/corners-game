using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntry : MonoBehaviour
    {
        [Header("MANAGERS")]
        [SerializeField]
        private GameplayViewManager viewManager;

        [SerializeField]
        private BoardManager boardManager;

        [Header("PROVIDERS")]
        [SerializeField]
        private BoardProvider boardProvider;

        [Header("CONFIGS")]
        [SerializeField]
        private BoardAssets boardAssets;

        private PoolController poolController;
        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            poolController = new PoolController(boardAssets, boardProvider);
        }

        public void Initialize()
        {
            var gameplayController = new GameplayController
            (
                viewManager,
                boardManager,
                boardAssets,
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