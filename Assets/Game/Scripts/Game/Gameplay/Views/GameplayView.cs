using System;
using Game.Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class GameplayView : ViewBase
    {
        [Header("TEXTS")]
        [SerializeField]
        private TMP_Text timerText;

        [SerializeField]
        private TMP_Text disconnectText;

        [SerializeField]
        private TMP_Text runText;

        [Header("BUTTONS")]
        [SerializeField]
        private SmartButton homeButton;

        private readonly CompositeDisposable disposable = new();
        private readonly ISubject<Unit> menuEvent = new Subject<Unit>();

        public IObservable<Unit> MenuEvent => menuEvent;

        private void OnEnable()
        {
            disposable.Clear();

            homeButton.ClickedEvent.Subscribe(OnMenu).AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
        }

        public void RemainingText(string time)
        {
            timerText.text = time;
        }

        public void DisconnectText(string text)
        {
            disconnectText.text = text;
        }

        public void PlayerText(string text)
        {
            runText.text = text.ToUpper();
        }

        // Events

        private void OnMenu(Unit unit)
        {
            menuEvent?.OnNext(unit);
        }
    }
}