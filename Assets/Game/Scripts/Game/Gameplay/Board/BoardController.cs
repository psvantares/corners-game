using System;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardController : IDisposable
    {
        private readonly Board board;
        private readonly BoardCellHighlight highlight;
        private readonly AIGame ai;

        private List<BoardCell> availableToMoveCells;
        private PlayerType activePlayer;

        public event Action<PlayerType> PlayerChanged;
        public event Action<PlayerType> PlayerWin;

        public BoardController(BoardProvider provider, BoardMode boardMode, bool aiOpponent, BoardResources resources, BoardConfig config)
        {
            board = new Board(provider, boardMode, resources, config);
            board.Initialize();

            highlight = new BoardCellHighlight(config.BoardSize.x + config.BoardSize.y, provider.HighlightTransform, resources);
            highlight.Initialize();

            if (aiOpponent)
            {
                ai = new AIGame(PlayerType.Black, board);
            }

            BoardInput.CellSelected += OnCellSelected;

            SetActivePlayer(PlayerType.White);
        }

        public void Dispose()
        {
            BoardInput.CellSelected -= OnCellSelected;

            board.Dispose();
            highlight.Dispose();
        }

        private void ProcessLogic(BoardCell cell)
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
                HighlightAvailableToMoveCells(availableToMoveCells);
            }
            else
            {
                if (availableToMoveCells == null || !availableToMoveCells.Contains(cell) || Checker.Selected == null)
                {
                    return;
                }

                highlight.Hide();
                MakeMove(cell);

                if (!IsWinner(activePlayer))
                {
                    SwitchPlayer();
                    if (IsAiTurn()) MakeAiMove();
                }
                else
                {
                    BoardInput.CellSelected -= OnCellSelected;
                    PlayerWin?.Invoke(activePlayer);
                }
            }
        }

        private void MakeAiMove()
        {
            MakeMove(ai.CalcTurn());
            SwitchPlayer();
        }

        private static void MakeMove(BoardCell boardCell)
        {
            Checker.Selected.SetPosition(boardCell.Position);
            Checker.Selected.SetSelected(false);
        }

        private bool IsAiTurn()
        {
            return ai != null && ai.Type == activePlayer;
        }

        private void HighlightAvailableToMoveCells(List<BoardCell> cells)
        {
            highlight.Show(cells);
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
            PlayerChanged?.Invoke(activePlayer);
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