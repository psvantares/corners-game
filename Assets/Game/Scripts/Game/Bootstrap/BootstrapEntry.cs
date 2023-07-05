using System;
using System.Collections.Generic;
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
            Initialize();
        }

        private void OnDestroy()
        {
            disposables.ForEach(x => x.Dispose());
            disposables.Clear();
        }

        private void Initialize()
        {
            disposables.Add(new BootstrapController(loaderView));
        }
    }
}