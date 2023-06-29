using System.Collections.Generic;
using System.Linq;
using Game.Data;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class AIGame
    {
        private readonly Board board;

        public PlayerType Type { get; }

        public AIGame(PlayerType type, Board board)
        {
            Type = type;
            this.board = board;
        }

        private static BoardCell GetNearestToTargetCell(BoardCell target, List<BoardCell> cells)
        {
            var result = cells.FirstOrDefault();
            var minDistance = float.MaxValue;

            foreach (var cell in cells)
            {
                var distance = Vector2Int.Distance(target.Position, cell.Position);

                if (!(distance < minDistance))
                {
                    continue;
                }

                minDistance = distance;
                result = cell;
            }

            return result;
        }

        private void SelectChecker()
        {
            var unit = board.GetRandomChecker(Type);
            unit.SetSelected(true);
        }

        public BoardCell CalcTurn()
        {
            var availableMoves = new List<BoardCell>();

            while (availableMoves.Count <= 0)
            {
                SelectChecker();
                var unitCell = board.GetCell(Checker.Selected.Position);
                availableMoves = board.GetAvailableMoves(unitCell);
            }

            var targets = board.GetEmptyStartPositions(Type == PlayerType.Black ? PlayerType.White : PlayerType.Black);
            var targetCell = targets[Random.Range(0, targets.Count)];
            return GetNearestToTargetCell(targetCell, availableMoves);
        }
    }
}