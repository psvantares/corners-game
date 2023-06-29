using System;
using UnityEngine;

namespace Game.Gameplay.Views
{
    public class NavigationView : ViewBase
    {
        [Header("VIEWS")]
        [SerializeField]
        private NavigationButtonView[] buttonView;

        public event Action<NavigationType> OnNavigationButton;

        public void OnEnable()
        {
            foreach (var view in buttonView)
            {
                view.OnClick += HandleClick;
            }
        }

        private void OnDisable()
        {
            foreach (var view in buttonView)
            {
                view.OnClick -= HandleClick;
            }
        }

        private void HandleClick(NavigationType type)
        {
            foreach (var view in buttonView)
            {
                view.SetActive(type == view.NavigationType);
            }

            OnNavigationButton?.Invoke(type);
        }
    }
}