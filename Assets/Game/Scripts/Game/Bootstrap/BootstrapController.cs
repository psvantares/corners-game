using System;
using Cysharp.Threading.Tasks;
using Game.Bootstrap.Views;
using UniRx;
using UnityEngine.SceneManagement;

namespace Game.Bootstrap
{
    public class BootstrapController : IDisposable
    {
        private readonly LoaderView loaderView;

        private readonly CompositeDisposable disposable = new();

        public BootstrapController(LoaderView loaderView)
        {
            this.loaderView = loaderView;

            Initialize();
        }

        public void Dispose()
        {
            disposable.Clear();
            disposable.Dispose();
        }

        private void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            loaderView.Initialize();
            loaderView.ChangeProgress(.25f);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            loaderView.ChangeProgress(0.55f);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            loaderView.ChangeProgress(0.75f);

            await LoadingCompleteAsync();
        }

        private async UniTask LoadingCompleteAsync()
        {
            loaderView.ChangeProgress(1f);

            var loadingScene = SceneManager.GetSceneByBuildIndex(0);

            if (loadingScene.isLoaded)
            {
                if (SceneManager.sceneCountInBuildSettings == 1)
                {
                    return;
                }

                const int nextSceneIndex = 1;
                var nextScene = GetSceneByIndex(nextSceneIndex);

                if (nextScene.IsValid())
                {
                    await UnloadBootstrapSceneAsync();
                }
                else
                {
                    await SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
                    nextScene = GetSceneByIndex(nextSceneIndex);

                    if (nextScene.isLoaded && nextScene.IsValid())
                    {
                        await UnloadBootstrapSceneAsync();
                    }
                }
            }
            else
            {
                loaderView.Clear();
            }
        }

        private async UniTask UnloadBootstrapSceneAsync()
        {
            await SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(0), UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            loaderView.Clear();
        }

        private static Scene GetSceneByIndex(int sceneIndex)
        {
            Scene nextScene = default;

            try
            {
                nextScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            }
            catch
            {
                // ignored
            }

            return nextScene;
        }
    }
}