using System.Collections.Generic;
using System.Linq;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class Board
    {
        private readonly List<BoardCell> cells = new();
        private readonly List<Checker> checkers = new();
        private readonly List<Checker> blackCheckers = new();
        private readonly List<Checker> whiteCheckers = new();

        private readonly BoardProvider provider;
        private readonly BoardMode boardMode;
        private readonly BoardResources resources;
        private readonly BoardConfig config;

        public Board(BoardProvider provider, BoardMode boardMode, BoardResources resources, BoardConfig config)
        {
            this.provider = provider;
            this.boardMode = boardMode;
            this.resources = resources;
            this.config = config;

            Initialize();
        }

        private void Initialize()
        {
            for (var x = 0; x < config.BoardSize.x; x++)
            {
                for (var y = 0; y < config.BoardSize.y; y++)
                {
                    var isWhite = (x + y) % 2 == 1;
                    var prefab = isWhite ? resources.BoardCellWhite : resources.BoardCellBlack;
                    var cell = Creator.Instantiate(prefab, new Vector2Int(x, y), provider.CellsTransform);
                    cell.SetPosition(x, y);
                    cells.Add(cell);
                }
            }

            foreach (var position in config.BlackPosition)
            {
                blackCheckers.Add(CreateChecker(PlayerType.Black, position));
            }

            foreach (var position in config.WhitePosition)
            {
                whiteCheckers.Add(CreateChecker(PlayerType.White, position));
            }

            checkers.AddRange(blackCheckers);
            checkers.AddRange(whiteCheckers);
        }

        private Checker CreateChecker(PlayerType type, Vector2Int position)
        {
            var isWhite = type == PlayerType.White;
            var prefab = isWhite ? resources.CheckerWhite : resources.CheckerBlack;
            var checker = Creator.Instantiate(prefab, position, isWhite ? provider.WhiteTransform : provider.BlackTransform);
            checker.SetPosition(position);
            return checker;
        }

        public Checker GetChecker(BoardCell boardCell)
        {
            return checkers.FirstOrDefault(u => u.Position == boardCell.Position);
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

        public BoardCell GetCell(Vector2Int position)
        {
            return cells.FirstOrDefault(c => c.Position == position);
        }

        private bool IsCellEmpty(BoardCell boardCell)
        {
            return GetChecker(boardCell) == null;
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

        public Checker GetRandomChecker(PlayerType playerType)
        {
            return playerType == PlayerType.Black
                ? blackCheckers[Random.Range(0, blackCheckers.Count)]
                : whiteCheckers[Random.Range(0, whiteCheckers.Count)];
        }

        public BoardCell GetRandomStartCell(PlayerType playerType)
        {
            return GetCell(playerType == PlayerType.Black
                ? config.BlackPosition[Random.Range(0, config.BlackPosition.Count)]
                : config.WhitePosition[Random.Range(0, config.WhitePosition.Count)]);
        }

        public List<BoardCell> GetEmptyStartPositions(PlayerType playerType)
        {
            var result = new List<BoardCell>();
            result.AddRange(playerType == PlayerType.Black
                ? config.BlackPosition.Select(GetCell).Where(cell => GetChecker(cell) == null)
                : config.WhitePosition.Select(GetCell).Where(cell => GetChecker(cell) == null));

            return result;
        }

        public List<BoardCell> GetAvailableMoves(BoardCell cell)
        {
            var stack = new Stack<BoardCell>();
            var result = new List<BoardCell>();
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
    }
}