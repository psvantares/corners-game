using UnityEngine;

namespace Game.Gameplay.Board
{
    [CreateAssetMenu(fileName = "BoardResources", menuName = "ScriptableObject/BoardResources", order = 1)]
    public class BoardResources : ScriptableObject
    {
        [Header("CELLS")]
        [SerializeField]
        private BoardCell boardCellBlack;

        [SerializeField]
        private BoardCell boardCellWhite;

        [Header("UNITS")]
        [SerializeField]
        private Checker checkerBlack;

        [SerializeField]
        private Checker checkerWhite;

        [Header("STAFF")]
        [SerializeField]
        private GameObject cellHighlighter;

        public BoardCell BoardCellBlack => boardCellBlack;
        public BoardCell BoardCellWhite => boardCellWhite;

        public Checker CheckerBlack => checkerBlack;
        public Checker CheckerWhite => checkerWhite;

        public GameObject CellHighlighter => cellHighlighter;
    }
}