using UnityEngine;

namespace Game.Gameplay.Board
{
    public class Checker : MonoBehaviour
    {
        [SerializeField]
        private PlayerType playerType;

        public PlayerType PlayerType => playerType;
        public static Checker Selected { get; private set; }

        private Vector3 normalScale, selectedScale;
        private const float SELECTED_SCALE_FACTOR = 1.25f;

        private void Start()
        {
            normalScale = transform.localScale;
            selectedScale = normalScale * SELECTED_SCALE_FACTOR;
        }

        public void SetPosition(Vector2Int position)
        {
            transform.localPosition = new Vector3(position.x, 0f, position.y);
        }

        public Vector2Int Position
        {
            get
            {
                var localPosition = transform.localPosition;
                return new Vector2Int((int)localPosition.x, (int)localPosition.z);
            }
        }

        public void SetSelected(bool value)
        {
            if (Selected != this)
            {
                if (Selected != null) Selected.SetSelected(false);
            }

            if (value)
            {
                Selected = this;
                transform.localScale = selectedScale;
            }
            else
            {
                Selected = null;
                transform.localScale = normalScale;
            }
        }
    }
}