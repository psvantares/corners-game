using System;
using System.Collections.Generic;
using Game.Bootstrap.Views;
using UnityEngine;

namespace Game.Bootstrap
{
    public class BootstrapEntry : MonoBehaviour
    {
        [SerializeField]
        private LoaderView loaderView;

        private readonly List<IDisposable> disposables = new();

        private void Awake()
        {
            var bootstrapController = new BootstrapController(loaderView);

            disposables.Add(bootstrapController);
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }
    }
}