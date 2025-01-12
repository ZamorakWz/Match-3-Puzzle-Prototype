using UnityEngine;

public class GridDrawer
{
    private Color gridColor;
    private float lineWidth;
    private float cellSize;
    private int columns;
    private int rows;

    public GridDrawer(Color gridColor, float lineWidth, float cellSize, int columns, int rows)
    {
        this.gridColor = gridColor;
        this.lineWidth = lineWidth;
        this.cellSize = cellSize;
        this.columns = columns;
        this.rows = rows;
    }

    public void DrawGridLines(Transform gridTransform)
    {
        //GameObject linesParent = new GameObject("GridLines");
        //linesParent.transform.SetParent(gridTransform, false);

        for (int y = 0; y <= rows; y++)
        {
            DrawLine(new Vector3(0, y * cellSize, 0), new Vector3(columns * cellSize, y * cellSize, 0), gridTransform);
        }

        for (int x = 0; x <= columns; x++)
        {
            DrawLine(new Vector3(x * cellSize, 0, 0), new Vector3(x * cellSize, rows * cellSize, 0), gridTransform);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Transform gridTransform)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.SetParent(gridTransform);
        LineRenderer line = lineObj.AddComponent<LineRenderer>();

        line.startColor = gridColor;
        line.endColor = gridColor;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 2;
        line.useWorldSpace = true;

        line.SetPosition(0, start);
        line.SetPosition(1, end);

        line.material = new Material(Shader.Find("Sprites/Default"));
    }
}
