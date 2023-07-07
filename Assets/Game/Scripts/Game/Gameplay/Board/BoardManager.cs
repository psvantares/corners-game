using UnityEngine;

namespace Game.Gameplay
{
    public class BoardManager : MonoBehaviour
    {
        [Header("ROOT")]
        [SerializeField]
        private GameObject boardObject;

        [SerializeField]
        private Transform backgroundTransform;

        [Space]
        [SerializeField]
        private BoardCamera boardCamera;

        public void Clear()
        {
            boardObject.SetActive(false);
        }

        public void StartGame(BoardContext context, bool isRotate)
        {
            SetupCamera(context.Config.BoardSize, isRotate);
            boardObject.SetActive(true);
        }

        private void SetupCamera(Vector2Int boardSize, bool isRotate)
        {
            boardCamera.Setup(boardSize, isRotate);

            var cameraPosition = boardCamera.transform.position;
            backgroundTransform.localPosition = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        }
    }
}