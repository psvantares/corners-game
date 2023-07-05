using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(fileName = "BoardAssets", menuName = "Configs/Board/BoardAssets", order = 1)]
    public class BoardAssets : ScriptableObject
    {
        [field: Header("DECKS"), SerializeField]
        public BoardConfig BoardConfigDiagonal { get; private set; }

        [field: SerializeField]
        public BoardConfig BoardConfigSquare { get; private set; }

        [field: Header("CELLS"), SerializeField]
        public Cell CellBlack { get; private set; }

        [field: SerializeField]
        public Cell CellWhite { get; private set; }

        [field: SerializeField]
        public CellHighlight CellHighlight { get; private set; }

        [field: Header("UNITS"), SerializeField]
        public Checker CheckerBlack { get; private set; }

        [field: SerializeField]
        public Checker CheckerWhite { get; private set; }
    }
}