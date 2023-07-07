using UnityEngine;

namespace Game.Gameplay
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        public void Setup(Vector2Int boardSize, bool isRotate)
        {
            var bounds = GetBoardBounds(boardSize);
            var width = bounds.size.x;
            var orthographicSize = 0.5f * width / boardCamera.aspect;

            boardCamera.orthographicSize = orthographicSize;

            var cameraTransform = boardCamera.transform;
            var position = cameraTransform.position;

            cameraTransform.position = new Vector3((boardSize.x / 2f) - 0.5f, (boardSize.y / 2f) - 0.5f, position.z);

            if (isRotate)
            {
                boardCamera.transform.localRotation = new Quaternion(0, 0, 180, 0);
            }
        }

        private static Bounds GetBoardBounds(Vector2Int boardSize)
        {
            const float step = 1.05f;
            var bounds = new Bounds();
            var offset = 0.75f * step * Vector2.one;
            var max = new Vector2(step * (boardSize.y - 1), step * (boardSize.x - 1)) + offset;
            var min = -offset;

            bounds.Encapsulate(max);
            bounds.Encapsulate(min);

            return bounds;
        }
    }
}