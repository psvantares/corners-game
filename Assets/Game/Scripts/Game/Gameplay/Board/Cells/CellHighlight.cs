using UnityEngine;

namespace Game.Gameplay.Board
{
    public class CellHighlight : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}