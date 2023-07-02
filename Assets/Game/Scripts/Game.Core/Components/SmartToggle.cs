using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class SmartToggle : MonoBehaviour
    {
        [SerializeField]
        protected Toggle MainToggle;

        [SerializeField]
        protected Transform AnimationTransform;

        [SerializeField]
        protected TMP_Text MainText;

        private bool isOn;
        private Sequence sequence;
        private readonly ISubject<bool> clickedEvent = new Subject<bool>();

        public IObservable<bool> ClickedEvent => clickedEvent;

        protected virtual void OnEnable()
        {
            MainToggle.onValueChanged.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            MainToggle.onValueChanged.RemoveListener(OnClick);
            sequence?.Kill();
        }

        public void SetMessage(string message)
        {
            if (MainText == null)
            {
                return;
            }

            MainText.text = message;
        }

        public void SetInteractable(bool isInteractable)
        {
            MainToggle.interactable = isInteractable;
        }

        public void SetActive(bool active)
        {
            MainToggle.gameObject.SetActive(active);
        }

        public void ForceClick(bool isOn)
        {
            MainToggle.isOn = isOn;
        }

        // Events

        protected virtual void OnClick(bool isOn)
        {
            if (this.isOn == isOn)
            {
                return;
            }

            this.isOn = isOn;

            if (AnimationTransform == null)
            {
                clickedEvent.OnNext(isOn);
            }
            else
            {
                sequence?.Kill();
                sequence = DOTween.Sequence();
                sequence.Append(AnimationTransform.DOScale(0.8f, 0.1f).SetEase(Ease.OutBack));
                sequence.Append(AnimationTransform.DOScale(1, 0.1f).SetEase(Ease.OutBack));
                sequence.OnComplete(() => clickedEvent.OnNext(isOn));
            }
        }
    }
}