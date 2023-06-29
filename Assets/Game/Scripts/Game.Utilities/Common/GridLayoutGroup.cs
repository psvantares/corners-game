using UnityEngine;

namespace Game.Utilities
{
    public class GridLayoutGroup : UnityEngine.UI.GridLayoutGroup
    {
        public override void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        private void SetCellsAlongAxis(int axis)
        {
            var rectChildrenCount = rectChildren.Count;

            if (axis == 0)
            {
                for (var i = 0; i < rectChildrenCount; i++)
                {
                    var rect = rectChildren[i];

                    m_Tracker.Add(this, rect,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.SizeDelta);

                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                    rect.sizeDelta = cellSize;
                }

                return;
            }

            var rect1 = rectTransform.rect;
            var width = rect1.size.x;
            var height = rect1.size.y;

            var cellCountX = 1;
            var cellCountY = 1;

            if (m_Constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;

                if (rectChildrenCount > cellCountX)
                    cellCountY = rectChildrenCount / cellCountX + (rectChildrenCount % cellCountX > 0 ? 1 : 0);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                cellCountY = m_ConstraintCount;

                if (rectChildrenCount > cellCountY)
                    cellCountX = rectChildrenCount / cellCountY + (rectChildrenCount % cellCountY > 0 ? 1 : 0);
            }
            else
            {
                cellCountX = cellSize.x + spacing.x <= 0 ? int.MaxValue : Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
                cellCountY = cellSize.y + spacing.y <= 0 ? int.MaxValue : Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }

            var cornerX = (int)startCorner % 2;
            var cornerY = (int)startCorner / 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            if (startAxis == Axis.Horizontal)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildrenCount);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildrenCount / (float)cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildrenCount);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectChildrenCount / (float)cellsPerMainAxis));
            }

            var lastCellsCount = rectChildrenCount % cellsPerMainAxis;

            var requiredSpace = new Vector2(
                actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
            );
            var startOffset = new Vector2(
                GetStartOffset(0, requiredSpace.x),
                GetStartOffset(1, requiredSpace.y)
            );

            var actualLastCellsCount = lastCellsCount == 0 ? cellsPerMainAxis : lastCellsCount;
            var cellsX = startAxis == Axis.Horizontal ? actualLastCellsCount : actualCellCountX;
            var cellsY = startAxis == Axis.Vertical ? actualLastCellsCount : actualCellCountY;

            var lastCellsRequiredSpace = new Vector2(
                cellsX * cellSize.x + (cellsX - 1) * spacing.x,
                cellsY * cellSize.y + (cellsY - 1) * spacing.y
            );

            var lastCellsStartOffset = new Vector2(
                GetStartOffset(0, lastCellsRequiredSpace.x),
                GetStartOffset(1, lastCellsRequiredSpace.y)
            );

            for (var i = 0; i < rectChildrenCount; i++)
            {
                int positionX;
                int positionY;
                var cellStartOffset = (i + 1 > rectChildrenCount - actualLastCellsCount) ? lastCellsStartOffset : startOffset;

                if (startAxis == Axis.Horizontal)
                {
                    positionX = i % cellsPerMainAxis;
                    positionY = i / cellsPerMainAxis;
                }
                else
                {
                    positionX = i / cellsPerMainAxis;
                    positionY = i % cellsPerMainAxis;
                }

                if (cornerX == 1)
                    positionX = actualCellCountX - 1 - positionX;
                if (cornerY == 1)
                    positionY = actualCellCountY - 1 - positionY;

                SetChildAlongAxis(rectChildren[i], 0, cellStartOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);
                SetChildAlongAxis(rectChildren[i], 1, cellStartOffset.y + (cellSize[1] + spacing[1]) * positionY, cellSize[1]);
            }
        }
    }
}