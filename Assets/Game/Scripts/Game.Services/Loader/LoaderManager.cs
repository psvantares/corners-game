using DG.Tweening;
using UnityEngine;

namespace Game.Services
{
    public class LoaderManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject rootObject;

        [SerializeField]
        private Transform loaderTransform;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private Tweener rotateTweener;
        private Tweener fadeTweener;

        private void Awake()
        {
            UpdateCanvas(false);
            UpdateRoot(false);
        }

        private void OnDisable()
        {
            rotateTweener?.Kill();
            fadeTweener?.Kill();
        }

        public void Show()
        {
            UpdateCanvas(true);
            UpdateRoot(true);
            FadeIn();
            PlayRotate();
        }

        public void Hide()
        {
            FadeOut();
        }

        private void FadeIn()
        {
            fadeTweener?.Kill();
            fadeTweener = canvasGroup
                .DOFade(1, 0.5f)
                .SetDelay(0.1f);
        }

        private void FadeOut()
        {
            fadeTweener?.Kill();
            fadeTweener = canvasGroup
                .DOFade(0, 0.5f)
                .SetDelay(0.5f)
                .OnComplete(() =>
                {
                    UpdateCanvas(false);
                    UpdateRoot(false);
                });
        }

        private void PlayRotate()
        {
            rotateTweener?.Kill();
            rotateTweener = loaderTransform
                .DOLocalRotate(new Vector3(0, 0, -360), 2f, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void UpdateCanvas(bool active)
        {
            canvasGroup.interactable = active;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.alpha = 0;
        }

        private void UpdateRoot(bool active)
        {
            rootObject.SetActive(active);
        }
    }
}