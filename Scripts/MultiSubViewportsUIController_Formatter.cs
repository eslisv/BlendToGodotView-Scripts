using Godot;
using System.Collections.Generic;

namespace AM.ModelViewerTool
{
    public sealed class MultiSubViewportsUIController_Formatter
    {
        private readonly List<Line2D> _viewportBorderLinesNonAlloc = new();
        private readonly IReadOnlyMultiSubViewportsUIController_Args _parentConrollerArgs;
        private Node _lineParentNode;
        private Color _borderLineColor;

        public MultiSubViewportsUIController_Formatter(IReadOnlyMultiSubViewportsUIController_Args parentControllerArgs, Node lineParentNode, Color borderLineColor)
        {
            _parentConrollerArgs = parentControllerArgs;
            _lineParentNode = lineParentNode;
            _borderLineColor = borderLineColor;
        }

        public void Refresh(FormatSubViewportsContext context)
        {
            ClearBorders();
            FormatGridViewports(context);
            if (context.GridDimensions.X <= 1 && context.GridDimensions.Y <= 1) { return; }
            FormatGridSeparatorLines(context);
        }

        private void ClearBorders()
        {
            foreach (Line2D line in _viewportBorderLinesNonAlloc)
            {
                line.QueueFree();
            }
            _viewportBorderLinesNonAlloc.Clear();
        }

        private void FormatGridViewports(FormatSubViewportsContext context)
        {
            const int MIN_GRID_DIMENSION = 1;
            const int MAX_GRID_DIMENSION = 4;

            Vector2I gridDimensions = context.GridDimensions;
            Vector2 viewportSize = context.ViewportSize;

            int stretchShrink = Mathf.Max(MAX_GRID_DIMENSION - gridDimensions.X, MAX_GRID_DIMENSION - gridDimensions.Y);
            stretchShrink = Mathf.Max(stretchShrink, MIN_GRID_DIMENSION);
            int xIndex = 0;
            int yIndex = 0;
            foreach (SubViewportUIController viewportController in _parentConrollerArgs.GetSubViewportControllers())
            {
                Vector2 position = new Vector2(viewportSize.X * xIndex, viewportSize.Y * yIndex);
                if (!viewportController.TryReformat(position, viewportSize, stretchShrink)) { continue; }

                ++xIndex;
                if (xIndex < gridDimensions.X) { continue; }
                xIndex = 0;
                ++yIndex;
            }
        }

        private void FormatGridSeparatorLines(FormatSubViewportsContext context)
        {
            Vector2I gridDimensions = context.GridDimensions;
            Vector2 viewportSize = context.ViewportSize;
            Vector2 screenSize = context.ScreenSize;

            Vector2I roundedViewportSize = new Vector2I(Mathf.RoundToInt(viewportSize.X), Mathf.RoundToInt(viewportSize.Y));
            for (int x = 1; x < gridDimensions.X; ++x)
            {
                Vector2 point1 = new Vector2(roundedViewportSize.X * x, 0);
                Vector2 point2 = new Vector2(roundedViewportSize.X * x, screenSize.Y);
                Line2D vertLine = CreateSeparatorLine(point1, point2);
                AddSeparatorLine(vertLine);
            }
            for (int y = 1; y < gridDimensions.Y; ++y)
            {
                Vector2 point1 = new Vector2(0, roundedViewportSize.Y * y);
                Vector2 point2 = new Vector2(screenSize.X, roundedViewportSize.Y * y);
                Line2D horiLine = CreateSeparatorLine(point1, point2);
                AddSeparatorLine(horiLine);
            }
        }

        private void AddSeparatorLine(Line2D separatorLine)
        {
            _lineParentNode.AddChild(separatorLine);
            _viewportBorderLinesNonAlloc.Add(separatorLine);
        }

        private Line2D CreateSeparatorLine(Vector2 point1, Vector2 point2)
        {
            Line2D separatorLine = new Line2D();
            Vector2[] vertLinePoints = {
                point1,
                point2
            };
            separatorLine.Points = vertLinePoints;
            separatorLine.DefaultColor = _borderLineColor;
            separatorLine.Width = 2;
            separatorLine.TopLevel = true;
            return separatorLine;
        }
    }
}