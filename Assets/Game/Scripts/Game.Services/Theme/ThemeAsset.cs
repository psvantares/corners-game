using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    [CreateAssetMenu(fileName = "ThemeAsset", menuName = "ScriptableObject/ThemeAsset", order = 1)]
    public class ThemeAsset : ScriptableObject
    {
        [SerializeField]
        private Theme[] theme;

        public IEnumerable<Theme> Theme => theme;
    }
}