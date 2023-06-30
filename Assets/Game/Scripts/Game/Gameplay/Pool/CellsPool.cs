using Game.Gameplay.Board;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Pool
{
    public class CellsPool : ObjectPool<Cell>
    {
        private readonly Cell cell;
        private readonly Transform parent;

        public CellsPool(Cell cell, Transform parent)
        {
            this.cell = cell;
            this.parent = parent;
        }

        protected override Cell CreateInstance()
        {
            var instance = Object.Instantiate(cell, parent, false);
            return instance;
        }
    }
}