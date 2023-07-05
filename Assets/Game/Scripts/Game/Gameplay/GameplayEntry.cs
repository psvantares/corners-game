using System;
using System.Collections.Generic;
using Game.Core;
using Game.Services;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayEntry : MonoBehaviour
    {
        [Header("MANAGERS")]
        [SerializeField]
        private BoardManager boardManager;

        [Header("PROVIDERS")]
        [SerializeField]
        private BoardProvider boardProvider;

        [Header("CONFIGS")]
        [SerializeField]
        private BoardAssets boardAssets;

        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            var poolController = new PoolController(boardAssets, boardProvider);
            var gameplayController = new GameplayController
            (
                boardManager,
                boardAssets,
                boardProvider,
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