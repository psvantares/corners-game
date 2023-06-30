using Game.Gameplay.Board;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Pool
{
    public class CellsHighlightPool : ObjectPool<CellHighlight>
    {
        private readonly CellHighlight cell;
        private readonly Transform parent;

        public CellsHighlightPool(CellHighlight cell, Transform parent)
        {
            this.cell = cell;
            this.parent = parent;
        }

        protected override CellHighlight CreateInstance()
        {
            var instance = Object.Instantiate(cell, parent, false);
            return instance;
        }
    }
}