using UnityEngine;

namespace Game.Gameplay.Board
{
    public class Cell : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        public void SetPosition(int x, int y)
        {
            Position = new Vector2Int(x, y);
            transform.localPosition = new Vector3(x, y, 0f);
        }
    }
}