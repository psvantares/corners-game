using System;
using Fusion;
using UnityEngine;

namespace Game.Gameplay
{
    public class BoardInput : NetworkBehaviour
    {
        [SerializeField]
        private Camera boardCamera;

        public static event Action<Vector2Int> CellSelected;

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false)
            {
                return;
            }

            if (Runner.LocalPlayer != Object.InputAuthority)
            {
                Object.AssignInputAuthority(Runner.LocalPlayer);
            }

            if (GetInput<NetworkInputData>(out var input) == false)
            {
                return;
            }

            if ((input.Buttons & NetworkInputData.MouseButton1) != 0)
            {
                var position = RaycastCellPosition(boardCamera, input);

                if (!position.HasValue)
                {
                    return;
                }

                CellSelected?.Invoke(position.Value);
            }
        }

        private static Vector2Int? RaycastCellPosition(Camera camera, NetworkInputData input)
        {
            var ray = camera.ScreenPointToRay(input.Position);

            if (!Physics.Raycast(ray, out var hit))
            {
                return null;
            }

            var position = hit.collider.transform.position;
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }
    }
}