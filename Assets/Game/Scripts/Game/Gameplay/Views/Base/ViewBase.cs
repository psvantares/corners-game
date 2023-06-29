﻿using UnityEngine;

namespace Game.Gameplay.Views
{
    public class ViewBase : SafeArea
    {
        [Header("ROOT")]
        [SerializeField]
        private GameObject root;

        [SerializeField]
        private NavigationType navigationType;

        public void SetActive(bool active)
        {
            if (root.activeSelf == active)
            {
                return;
            }

            root.SetActive(active);
        }

        public void SetActive(NavigationType type)
        {
            root.SetActive(type == navigationType);
        }
    }
}