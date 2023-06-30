using System;
using System.Collections.Generic;
using Game.Data;
using Game.Gameplay.Board;

namespace Game.Gameplay.Pool
{
    public class PoolController : IDisposable
    {
        private readonly List<Cell> cellsWhite = new();
        private readonly List<Cell> cellsBlack = new();
        private readonly CellsPool cellsWhitePool;
        private readonly CellsPool cellsBlackPool;

        private readonly List<CellHighlight> cellsHighlights = new();
        private readonly CellsHighlightPool cellsHighlightPool;

        private readonly List<Checker> checkersWhite = new();
        private readonly List<Checker> checkersBlack = new();
        private readonly CheckersPool checkersWhitePool;
        private readonly CheckersPool checkersBlackPool;

        public PoolController(BoardAssets assets, BoardProvider provider)
        {
            // Cells
            cellsWhitePool = new CellsPool(assets.CellWhite, provider.CellsWhiteTransform);
            cellsBlackPool = new CellsPool(assets.CellBlack, provider.CellsBlackTransform);

            // Highlight
            cellsHighlightPool = new CellsHighlightPool(assets.CellHighlight, provider.CellsHighlightTransform);

            // Checkers
            checkersWhitePool = new CheckersPool(assets.CheckerWhite, provider.CheckerWhiteTransform);
            checkersBlackPool = new CheckersPool(assets.CheckerBlack, provider.CheckerBlackTransform);
        }

        public void Dispose()
        {
        }

        public void Clear()
        {
            ReturnAll();
        }

        public Checker GetCheckerPrefab(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.White:
                    var checkerWhite = checkersWhitePool.Rent();
                    checkersWhite.Add(checkerWhite);
                    return checkerWhite;
                case PlayerType.Black:
                    var checkerBlack = checkersBlackPool.Rent();
                    checkersBlack.Add(checkerBlack);
                    return checkerBlack;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
            }
        }

        public Cell GetCellPrefab(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.White:
                    var cellWhite = cellsWhitePool.Rent();
                    cellsWhite.Add(cellWhite);
                    return cellWhite;
                case CellType.Black:
                    var cellBlack = cellsBlackPool.Rent();
                    cellsBlack.Add(cellBlack);
                    return cellBlack;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
        }

        public CellHighlight GetHighlightPrefab()
        {
            var highlight = cellsHighlightPool.Rent();
            cellsHighlights.Add(highlight);
            return highlight;
        }

        private void ReturnAll()
        {
            ReturnAllCheckers();
            ReturnAllHighlights();
            ReturnAllCells();
        }

        private void ReturnAllCells()
        {
            foreach (var cell in cellsWhite)
            {
                cellsWhitePool.Return(cell);
            }

            foreach (var cell in cellsBlack)
            {
                cellsBlackPool.Return(cell);
            }

            cellsWhite.Clear();
            cellsBlack.Clear();
        }

        private void ReturnAllHighlights()
        {
            foreach (var cell in cellsHighlights)
            {
                cellsHighlightPool.Return(cell);
            }

            cellsHighlights.Clear();
        }

        private void ReturnAllCheckers()
        {
            foreach (var checker in checkersWhite)
            {
                checkersWhitePool.Return(checker);
            }

            foreach (var checker in checkersBlack)
            {
                checkersBlackPool.Return(checker);
            }

            checkersWhite.Clear();
            checkersBlack.Clear();
        }
    }
}