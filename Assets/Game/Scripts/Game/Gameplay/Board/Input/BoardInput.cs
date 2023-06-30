using System;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        public Camera BoardCamera => boardCamera;

        public static event Action<Vector2Int> CellSelected;

        public void Update()
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                return;
            }

            var position = RaycastCellPosition(boardCamera);

            if (!position.HasValue)
            {
                return;
            }

            CellSelected?.Invoke(position.Value);
        }

        private static Vector2Int? RaycastCellPosition(Camera camera)
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit))
            {
                return null;
            }

            var position = hit.collider.transform.position;
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }
    }
}