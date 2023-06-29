using System;
using System.Collections.Generic;
using Game.Gameplay.Board;
using Game.Gameplay.Theme;
using Game.Gameplay.Views;
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

        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            var gameplayController = new GameplayController(viewManager, boardManager, themeManager);

            disposables.Add(gameplayController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }
    }
}