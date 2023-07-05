using UnityEngine;

namespace Game.Gameplay
{
    public class BoardProvider : MonoBehaviour
    {
        [field: SerializeField]
        public Transform BoardTransform { get; private set; }

        [field: Space, SerializeField]
        public Transform CellsWhiteTransform { get; private set; }

        [field: SerializeField]
        public Transform CellsBlackTransform { get; private set; }

        [field: SerializeField]
        public Transform CellsHighlightTransform { get; private set; }

        [field: Space, SerializeField]
        public Transform CheckerWhiteTransform { get; private set; }

        [field: SerializeField]
        public Transform CheckerBlackTransform { get; private set; }
    }
}