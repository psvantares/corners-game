using UnityEngine;

namespace Game.Gameplay
{
    public class CellHighlight : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}