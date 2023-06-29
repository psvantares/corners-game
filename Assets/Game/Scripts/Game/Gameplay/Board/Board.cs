using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class Board
    {
        private readonly List<BoardCell> cells = new();
        private readonly List<Checker> checkers = new();
        private readonly List<Checker> blackCheckers = new();
        private readonly List<Checker> whiteCheckers = new();

        private readonly Transform boardTransform;
        private readonly BoardMode boardMode;
        private readonly BoardResources boardResources;
        private readonly BoardConfig boardConfig;

        public Board(Transform boardTransform, BoardMode boardMode, BoardResources boardResources, BoardConfig boardConfig)
        {
            this.boardTransform = boardTransform;
            this.boardMode = boardMode;
            this.boardResources = boardResources;
            this.boardConfig = boardConfig;

            Initialize();
        }

        private void Initialize()
        {
            for (var x = 0; x < boardConfig.BoardSize.x; x++)
            {
                for (var y = 0; y < boardConfig.BoardSize.y; y++)
                {
                    var prefab = (x + y) % 2 == 1 ? boardResources.BoardCellWhite : boardResources.BoardCellBlack;
                    var cell = Utils.Instantiate(prefab, new Vector2Int(x, y), boardTransform);
                    cell.SetPosition(x, y);
                    cells.Add(cell);
                }
            }

            foreach (var position in boardConfig.BlackPosition)
            {
                blackCheckers.Add(CreateChecker(PlayerType.Black, position));
            }

            foreach (var position in boardConfig.WhitePosition)
            {
                whiteCheckers.Add(CreateChecker(PlayerType.White, position));
            }

            checkers.AddRange(blackCheckers);
            checkers.AddRange(whiteCheckers);
        }

        private Checker CreateChecker(PlayerType type, Vector2Int position)
        {
            var prefab = type == PlayerType.Black ? boardResources.CheckerBlack : boardResources.CheckerWhite;
            var checker = Utils.Instantiate(prefab, position, boardTransform);
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
                PlayerType.Black => blackCheckers.All(unit => boardConfig.WhitePosition.Contains(unit.Position)),
                PlayerType.White => whiteCheckers.All(unit => boardConfig.BlackPosition.Contains(unit.Position)),
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
            return position.x < 0 || position.x >= boardConfig.BoardSize.x || position.y < 0 || position.y >= boardConfig.BoardSize.y;
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
                ? boardConfig.BlackPosition[Random.Range(0, boardConfig.BlackPosition.Count)]
                : boardConfig.WhitePosition[Random.Range(0, boardConfig.WhitePosition.Count)]);
        }

        public List<BoardCell> GetEmptyStartPositions(PlayerType playerType)
        {
            var result = new List<BoardCell>();
            result.AddRange(playerType == PlayerType.Black
                ? boardConfig.BlackPosition.Select(GetCell).Where(cell => GetChecker(cell) == null)
                : boardConfig.WhitePosition.Select(GetCell).Where(cell => GetChecker(cell) == null));

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