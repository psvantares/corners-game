using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardCell : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        public void SetPosition(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}