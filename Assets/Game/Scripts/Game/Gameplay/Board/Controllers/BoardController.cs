using System;
using System.Collections.Generic;
using Game.Data;
using UniRx;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardController : IDisposable
    {
        private Board board;
        private BotGame bot;

        private List<Cell> availableToMoveCells;
        private List<CellHighlight> cellsHighlight;
        private PlayerType activePlayer;

        private readonly ISubject<PlayerType> playerChangedEvent = new Subject<PlayerType>();
        private readonly ISubject<PlayerType> playerWinEvent = new Subject<PlayerType>();

        public IObservable<PlayerType> PlayerChangedEvent => playerChangedEvent;
        public IObservable<PlayerType> PlayerWinEvent => playerWinEvent;

        public BoardController(BoardContext context)
        {
            InitializeBoard(context);
            InitializeBot(context);
            InitializeHighlight(context);

            Subscribes();
            SetActivePlayer(PlayerType.White);
        }

        public void Dispose()
        {
            Unsubscribes();

            board.Dispose();

            cellsHighlight.Clear();
            availableToMoveCells.Clear();
        }

        private void InitializeBoard(BoardContext context)
        {
            board = new Board(context);
            board.Initialize();
        }

        private void InitializeBot(BoardContext context)
        {
            if (context.GameModel.GameMode == GameMode.Bot)
            {
                bot = new BotGame(PlayerType.Black, board);
            }
        }

        private void InitializeHighlight(BoardContext context)
        {
            cellsHighlight = new List<CellHighlight>();
            availableToMoveCells = new List<Cell>();

            var max = context.Config.BoardSize.x + context.Config.BoardSize.y;
            var pool = context.Pool;

            for (var i = 0; i < max; i++)
            {
                var cellHighlight = pool.GetHighlightPrefab();
                cellHighlight.SetActive(false);
                cellsHighlight.Add(cellHighlight);
            }
        }

        private void Subscribes()
        {
            BoardInput.CellSelected += OnCellSelected;
        }

        private void Unsubscribes()
        {
            BoardInput.CellSelected -= OnCellSelected;
        }

        private void ShowHighlight(IReadOnlyList<Cell> cells)
        {
            HideHighlight();

            for (var i = 0; i < cells.Count; i++)
            {
                var position = cells[i].Position;
                var cellHighlight = cellsHighlight[i];
                cellHighlight.transform.localPosition = new Vector3(position.x, position.y, 0);
                cellHighlight.SetActive(true);
            }
        }

        private void HideHighlight()
        {
            foreach (var cellHighlight in cellsHighlight)
            {
                cellHighlight.SetActive(false);
            }
        }

        private void ProcessLogic(Cell cell)
        {
            if (IsAiTurn())
            {
                return;
            }

            var checker = board.GetChecker(cell);

            if (checker != null)
            {
                if (checker.PlayerType != activePlayer)
                {
                    return;
                }

                checker.SetSelected(true);
                availableToMoveCells = board.GetAvailableMoves(cell);
                ShowHighlight(availableToMoveCells);
            }
            else
            {
                if (availableToMoveCells == null || !availableToMoveCells.Contains(cell) || Checker.Selected == null)
                {
                    return;
                }

                HideHighlight();
                MakeMove(cell);

                if (!IsWinner(activePlayer))
                {
                    SwitchPlayer();

                    if (IsAiTurn())
                    {
                        MakeAiMove();
                    }
                }
                else
                {
                    Unsubscribes();
                    playerWinEvent?.OnNext(activePlayer);
                }
            }
        }

        private void MakeAiMove()
        {
            MakeMove(bot.CalcTurn());
            SwitchPlayer();
        }

        private static void MakeMove(Cell cell)
        {
            Checker.Selected.SetPosition(cell.Position);
            Checker.Selected.SetSelected(false);
        }

        private bool IsAiTurn()
        {
            return bot != null && bot.Type == activePlayer;
        }

        private void SwitchPlayer()
        {
            SetActivePlayer(NextPlayer());
        }

        private PlayerType NextPlayer()
        {
            return activePlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
        }

        private void SetActivePlayer(PlayerType playerType)
        {
            activePlayer = playerType;
            playerChangedEvent?.OnNext(activePlayer);
        }

        private bool IsWinner(PlayerType player)
        {
            return board.IsPlayerOnOpponentPositions(player == PlayerType.Black ? PlayerType.Black : PlayerType.White);
        }

        // Events

        private void OnCellSelected(Vector2Int position)
        {
            var cell = board.GetCell(position);

            if (cell == null)
            {
                return;
            }

            ProcessLogic(cell);
        }
    }
}