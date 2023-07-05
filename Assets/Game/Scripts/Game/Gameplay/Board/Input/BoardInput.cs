using System;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        private static readonly ISubject<Vector2Int> CellSelected = new Subject<Vector2Int>();

        public Camera BoardCamera => boardCamera;
        public static IObservable<Vector2Int> CellSelectedEvent => CellSelected;

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

            CellSelected?.OnNext(position.Value);
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