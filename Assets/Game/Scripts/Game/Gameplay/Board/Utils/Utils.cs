using UnityEngine;

namespace Game.Gameplay.Board
{
    public static class Utils
    {
        public static T Instantiate<T>(T original, Vector2Int position, Transform parent) where T : MonoBehaviour
        {
            var copy = Object.Instantiate(original, parent);
            copy.transform.localPosition = new Vector3(position.x, 0, position.y);
            return copy;
        }

        public static GameObject Instantiate(GameObject original, Vector2Int position, Transform parent)
        {
            var copy = Object.Instantiate(original, parent);
            copy.transform.localPosition = new Vector3(position.x, 0, position.y);
            return copy;
        }
    }
}