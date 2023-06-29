using UnityEngine;

namespace Game.Utilities
{
    public static class Creator
    {
        public static T Instantiate<T>(T original, Vector2Int position, Transform parent) where T : MonoBehaviour
        {
            var copy = Object.Instantiate(original, parent);
            copy.transform.localPosition = new Vector3(position.x, position.y, 0);
            return copy;
        }

        public static GameObject Instantiate(GameObject original, Vector2Int position, Transform parent)
        {
            var copy = Object.Instantiate(original, parent);
            copy.transform.localPosition = new Vector3(position.x, position.y, 0);
            return copy;
        }
    }
}