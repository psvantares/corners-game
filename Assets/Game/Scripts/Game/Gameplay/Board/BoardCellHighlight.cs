using System;
using System.Collections.Generic;
using Game.Utilities;
using UnityEngine;

namespace Game.Gameplay.Board
{
    public class BoardCellHighlight : IDisposable
    {
        private readonly List<GameObject> objects;

        public BoardCellHighlight(int max, Transform boardHighlightTransform, BoardResources boardResources)
        {
            objects = new List<GameObject>(max);

            for (var i = 0; i < max; i++)
            {
                objects.Add(Creator.Instantiate(boardResources.CellHighlighter, Vector2Int.zero, boardHighlightTransform));
            }
        }

        public void Show(List<BoardCell> cells)
        {
            Hide();

            for (var i = 0; i < cells.Count; i++)
            {
                var position = cells[i].Position;
                var go = objects[i];
                go.transform.localPosition = new Vector3(position.x, position.y, 0);
                go.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (var gameObject in objects)
            {
                gameObject.SetActive(false);
            }
        }

        public void Dispose()
        {
            objects.Clear();
        }
    }
}