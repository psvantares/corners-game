using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardProvider : MonoBehaviour
    {
        [field: SerializeField]
        public Transform BoardTransform { get; private set; }

        [field: SerializeField]
        public Transform CellsTransform { get; private set; }

        [field: SerializeField]
        public Transform WhiteTransform { get; private set; }

        [field: SerializeField]
        public Transform BlackTransform { get; private set; }

        [field: SerializeField]
        public Transform HighlightTransform { get; private set; }
    }
}