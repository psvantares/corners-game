using UnityEngine;

namespace Game.Gameplay.Board
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
        private BoardInput boardInput;

        [SerializeField]
        private BoardCamera boardCamera;

        private void Update()
        {
            boardInput.Update();
        }

        public void Clear()
        {
            boardObject.SetActive(false);
        }

        public void StartGame(BoardContext context)
        {
            SetupCamera(context.Config.BoardSize);
            boardObject.SetActive(true);
        }

        private void SetupCamera(Vector2Int boardSize)
        {
            boardCamera.Setup(boardSize);

            var cameraPosition = boardCamera.transform.position;
            backgroundTransform.localPosition = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        }
    }
}