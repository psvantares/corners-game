using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Board
{
    [CreateAssetMenu(fileName = "BoardConfig", menuName = "ScriptableObject/BoardConfig", order = 1)]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField]
        private List<Vector2Int> whitePosition;

        [SerializeField]
        private List<Vector2Int> blackPosition;

        [SerializeField]
        private Vector2Int boardSize;

        public List<Vector2Int> WhitePosition => whitePosition;
        public List<Vector2Int> BlackPosition => blackPosition;
        public Vector2Int BoardSize => boardSize;
    }
}