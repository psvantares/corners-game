using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core
{
    public class SmartButton : MonoBehaviour
    {
        [SerializeField]
        protected Button MainButton;

        [SerializeField]
        protected Transform AnimationTransform;

        [SerializeField]
        protected TMP_Text MainText;

        private Sequence sequence;
        private readonly ISubject<Unit> clickedEvent = new Subject<Unit>();

        public IObservable<Unit> ClickedEvent => clickedEvent;

        protected virtual void OnEnable()
        {
            MainButton.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            MainButton.onClick.RemoveListener(OnClick);
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
            MainButton.interactable = isInteractable;
        }

        public void SetActive(bool active)
        {
            MainButton.gameObject.SetActive(active);
        }

        // Events

        protected virtual void OnClick()
        {
            if (AnimationTransform == null)
            {
                clickedEvent.OnNext(Unit.Default);
            }
            else
            {
                sequence?.Kill();
                sequence = DOTween.Sequence();
                sequence.Append(AnimationTransform.DOScale(0.8f, 0.1f).SetEase(Ease.OutBack));
                sequence.Append(AnimationTransform.DOScale(1, 0.1f).SetEase(Ease.OutBack));
                sequence.OnComplete(() => clickedEvent.OnNext(Unit.Default));
            }
        }
    }
}