using System;
using Game.Core;
using Game.Data;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class CompleteView : ViewBase
    {
        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton continueButton;

        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text titleText;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> continueEvent = new Subject<Unit>();

        public IObservable<Unit> ContinueEvent => continueEvent;

        public void OnEnable()
        {
            disposable.Clear();

            continueButton.ClickedEvent.Subscribe(OnContinue).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void SetTitleText(PlayerType playerType)
        {
            titleText.text = $"Congrats: {playerType}";
        }

        // Events

        private void OnContinue(Unit unit)
        {
            continueEvent?.OnNext(unit);
        }
    }
}