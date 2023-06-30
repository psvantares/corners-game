using Game.Gameplay.Data;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;


        public void Setup(Vector2Int boardSize)
        {
            var bounds = GetBoardBounds(boardSize);
            var width = bounds.size.x;
            var orthographicSize = 0.5f * width / boardCamera.aspect;

            boardCamera.orthographicSize = orthographicSize;

            var cameraTransform = boardCamera.transform;
            var position = cameraTransform.position;

            cameraTransform.position = new Vector3((boardSize.x / 2f) - 0.5f, (boardSize.y / 2f) - 0.5f, position.z);
        }

        public Vector2 GetWorldPoint(Vector2 screenPoint)
        {
            return boardCamera.ScreenToWorldPoint(screenPoint);
        }

        private static Bounds GetBoardBounds(Vector2Int boardSize)
        {
            var bounds = new Bounds();
            var offset = 0.75f * Constants.CellStep * Vector2.one;
            var max = new Vector2(Constants.CellStep * (boardSize.y - 1), Constants.CellStep * (boardSize.x - 1)) + offset;
            var min = -offset;

            bounds.Encapsulate(max);
            bounds.Encapsulate(min);

            return bounds;
        }
    }
}