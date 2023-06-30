using Game.Data;
using Game.Models;
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
        private BoardConfig boardConfigDiagonal;

        [SerializeField]
        private BoardConfig boardConfigSquare;

        [Space]
        [SerializeField]
        private BoardInput boardInput;

        [SerializeField]
        private BoardCamera boardCamera;

        [SerializeField]
        private BoardProvider boardTransform;

        private IGameModel gameModel;
        private BoardController boardController;

        private void Update()
        {
            boardInput.Update();
        }

        public void Initialize(IGameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void Clear()
        {
            boardController.Dispose();
        }

        public void StartGame(BoardMode boardMode, bool aiOpponent)
        {
            var config = gameModel.DeckType == BoardDeckType.Diagonal ? boardConfigDiagonal : boardConfigSquare;
            boardController = new BoardController(boardTransform, boardMode, aiOpponent, boardResources, config);

            SetupCamera(config.BoardSize);

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