using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        private BoardResources boardResources;

        [SerializeField]
        private BoardConfig boardConfig;

        [SerializeField]
        private BoardInput boardInput;

        [SerializeField]
        private Transform boardTransform;

        [SerializeField]
        private Transform boardHighlightTransform;

        private BoardController cornersController;

        private void Start()
        {
            SetupCamera();
        }

        private void Update()
        {
            boardInput.Update();
        }

        public void StartGame(BoardMode boardMode, bool aiOpponent)
        {
            cornersController = new BoardController(boardTransform, boardHighlightTransform, boardMode, aiOpponent, boardResources, boardConfig);
        }

        public void PauseGame(bool value)
        {
            cornersController.Pause(value);
        }

        private void SetupCamera()
        {
            var cameraTransform = boardInput.BoardCamera.transform;
            var position = cameraTransform.position;
            var boardSize = boardConfig.BoardSize;

            cameraTransform.position = new Vector3((boardSize.x / 2f) - .5f, position.y, (boardSize.y / 2f) - .5f);
        }
    }
}