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
        private BoardResources boardResources;

        [SerializeField]
        private BoardConfig boardConfig;

        [SerializeField]
        private BoardInput boardInput;

        [SerializeField]
        private BoardCamera boardCamera;

        [SerializeField]
        private BoardProvider boardTransform;

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
            cornersController = new BoardController(boardTransform, boardMode, aiOpponent, boardResources, boardConfig);
            boardObject.SetActive(true);
        }

        public void PauseGame(bool value)
        {
            cornersController.Pause(value);
        }

        private void SetupCamera()
        {
            boardCamera.Setup(boardConfig.BoardSize);

            var cameraPosition = boardCamera.transform.position;
            backgroundTransform.localPosition = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        }
    }
}