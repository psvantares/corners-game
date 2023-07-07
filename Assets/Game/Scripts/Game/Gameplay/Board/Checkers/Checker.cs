using Game.Data;
using UnityEngine;

namespace Game.Gameplay
{
    public class Checker : MonoBehaviour
    {
        [SerializeField]
        private PlayerType playerType;

        private Vector3 normalScale, selectedScale;
        private const float SELECTED_SCALE_FACTOR = 1.14f;

        public PlayerType PlayerType => playerType;
        public int Index { get; private set; }


        public Vector2Int Position
        {
            get
            {
                var localPosition = transform.localPosition;
                return new Vector2Int((int)localPosition.x, (int)localPosition.y);
            }
        }

        public static Checker Selected { get; private set; }

        private void Start()
        {
            normalScale = transform.localScale;
            selectedScale = normalScale * SELECTED_SCALE_FACTOR;
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void SetPosition(Vector2Int position)
        {
            transform.localPosition = new Vector3(position.x, position.y, 0f);
        }

        public void SetSelected(bool value)
        {
            if (Selected != this)
            {
                if (Selected != null)
                {
                    Selected.SetSelected(false);
                }
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