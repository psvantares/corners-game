using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Bootstrap.Views
{
    public class LoaderView : MonoBehaviour
    {
        [SerializeField]
        private Slider progressSlider;

        [SerializeField]
        private bool usePercents;

        [SerializeField]
        private TMP_Text percentText;

        [SerializeField]
        private float speed = 0.5f;

        private IDisposable updateLoadingProgress;

        private float percentValue;
        private float oldValue;
        private float progress = 1f;

        public void Initialize()
        {
            percentText.gameObject.SetActive(usePercents);
            progressSlider.value = 0;
            percentText.text = "0%";
            percentValue = 0f;
            oldValue = 0f;
            progress = 1f;

            updateLoadingProgress = Observable.EveryUpdate().Subscribe(UpdateLoadingProgress);
        }

        public void Clear()
        {
            updateLoadingProgress?.Dispose();
            updateLoadingProgress = null;
        }

        public void ChangeProgress(float percent)
        {
            percentValue = oldValue;
            oldValue = percent;
            progress = percent;
        }

        private void UpdateLoadingProgress(long frame)
        {
            percentValue = Mathf.Lerp(percentValue, progress, Time.deltaTime * speed);
            percentText.text = $"{(int)(percentValue * 100f)}%";
            progressSlider.value = percentValue;
        }
    }
}