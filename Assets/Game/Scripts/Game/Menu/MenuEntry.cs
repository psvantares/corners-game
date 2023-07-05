using System;
using System.Collections.Generic;
using Fusion;
using Game.Core;
using Game.Data;
using Game.Services;
using UnityEngine;

namespace Game.Menu
{
    public class MenuEntry : MonoBehaviour
    {
        [Header("MANAGERS")]
        [SerializeField]
        private MenuViewManager menuViewManager;

        [Header("PREFABS")]
        [SerializeField]
        private PlayerData playerDataPrefab;

        [SerializeField]
        private NetworkRunner networkRunnerPrefab;


        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            var menuController = new MenuController
            (
                menuViewManager,
                playerDataPrefab,
                networkRunnerPrefab
            );

            disposables.Add(menuController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }
    }
}