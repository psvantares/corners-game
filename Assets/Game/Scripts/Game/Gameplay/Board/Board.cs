using System;
using System.Collections.Generic;
using System.Linq;
using Game.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay
{
    public class Board : IDisposable
    {
        private readonly List<Cell> cells = new();
        private readonly List<Checker> checkers = new();
        private readonly List<Checker> blackCheckers = new();
        private readonly List<Checker> whiteCheckers = new();

        private readonly BoardMode boardMode;
        private readonly BoardConfig config;
        private readonly PoolController pool;

        public Board(BoardContext context)
        {
            boardMode = context.GameModel.BoardMode;
            config = context.Config;
            pool = context.Pool;
        }

        public void Initialize()
        {
            for (var x = 0; x < config.BoardSize.x; x++)
            {
                for (var y = 0; y < config.BoardSize.y; y++)
                {
                    var isWhite = (x + y) % 2 == 1;
                    var cell = isWhite ? pool.GetCellPrefab(CellType.White) : pool.GetCellPrefab(CellType.Black);
                    cell.SetPosition(x, y);
                    cells.Add(cell);
                }
            }

            foreach (var position in config.BlackPosition)
            {
                var checker = pool.GetCheckerPrefab(PlayerType.Black);
                checker.SetPosition(position);
                blackCheckers.Add(checker);
            }

            foreach (var position in config.WhitePosition)
            {
                var checker = pool.GetCheckerPrefab(PlayerType.White);
                checker.SetPosition(position);
                whiteCheckers.Add(checker);
            }

            checkers.AddRange(blackCheckers);
            checkers.AddRange(whiteCheckers);
        }

        public void Dispose()
        {
            cells.Clear();
            checkers.Clear();
            blackCheckers.Clear();
            whiteCheckers.Clear();
        }

        public Checker GetChecker(Cell cell)
        {
            return checkers.FirstOrDefault(u => u.Position == cell.Position);
        }

        public bool IsPlayerOnOpponentPositions(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.Black => blackCheckers.All(unit => config.WhitePosition.Contains(unit.Position)),
                PlayerType.White => whiteCheckers.All(unit => config.BlackPosition.Contains(unit.Position)),
                _ => false
            };
        }

        public Cell GetCell(Vector2Int position)
        {
            return cells.FirstOrDefault(c => c.Position == position);
        }

        public List<Cell> GetEmptyStartPositions(PlayerType playerType)
        {
            var result = new List<Cell>();
            result.AddRange(playerType == PlayerType.Black
                ? config.BlackPosition.Select(GetCell).Where(cell => GetChecker(cell) == null)
                : config.WhitePosition.Select(GetCell).Where(cell => GetChecker(cell) == null));

            return result;
        }

        public List<Cell> GetAvailableMoves(Cell cell)
        {
            var stack = new Stack<Cell>();
            var result = new List<Cell>();
            stack.Push(cell);
            var skipEmpty = false;

            while (stack.Count > 0)
            {
                cell = stack.Pop();

                for (var x = -1; x <= 1; x++)
                {
                    for (var y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;

                        switch (boardMode)
                        {
                            case BoardMode.VerticalHorizontal when Mathf.Abs(x) == Mathf.Abs(y):
                            case BoardMode.Diagonal when Mathf.Abs(x) != Mathf.Abs(y):
                                continue;
                        }

                        var nextPosition = new Vector2Int(cell.Position.x + x, cell.Position.y + y);

                        if (IsPositionOutOfBounds(nextPosition))
                        {
                            continue;
                        }

                        if (IsCellEmpty(nextPosition))
                        {
                            if (!skipEmpty) result.Add(GetCell(nextPosition));
                        }
                        else
                        {
                            if (boardMode == BoardMode.Normal)
                            {
                                continue;
                            }

                            nextPosition = new Vector2Int(cell.Position.x + 2 * x, cell.Position.y + 2 * y);

                            if (IsPositionOutOfBounds(nextPosition))
                            {
                                continue;
                            }

                            if (!IsCellEmpty(nextPosition))
                            {
                                continue;
                            }

                            var c1 = GetCell(nextPosition);

                            if (result.Contains(c1))
                            {
                                continue;
                            }

                            result.Add(c1);
                            stack.Push(c1);
                        }
                    }
                }

                skipEmpty = true;
            }

            return result;
        }

        public Checker GetRandomChecker(PlayerType playerType)
        {
            return playerType == PlayerType.Black
                ? blackCheckers[Random.Range(0, blackCheckers.Count)]
                : whiteCheckers[Random.Range(0, whiteCheckers.Count)];
        }

        private bool IsCellEmpty(Cell cell)
        {
            return GetChecker(cell) == null;
        }

        private bool IsCellEmpty(Vector2Int position)
        {
            var cell = GetCell(position);
            return IsCellEmpty(cell);
        }

        private bool IsPositionOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x >= config.BoardSize.x || position.y < 0 || position.y >= config.BoardSize.y;
        }
    }
}