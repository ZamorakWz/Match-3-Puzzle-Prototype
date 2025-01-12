using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int columns;
    public int Columns => columns;
    [SerializeField] private int rows;
    public int Rows => rows;

    private float cellSize = 1f;
    public float CellSize => cellSize;

    [SerializeField] private Color gridColor;
    [SerializeField] private float lineWidth;

    private Transform[,] grid;
    private GridDrawer gridDrawer;
    private GroupMaker groupMaker;

    private List<List<BlockAbstract>> allGroups;

    [Inject] private BlockSpawnManager _blockSpawnManager;

    void Awake()
    {
        gridDrawer = new GridDrawer(gridColor, lineWidth, cellSize, columns, rows);
        groupMaker = new GroupMaker(this);

        if (gridDrawer == null)
        {
            Debug.LogError("GridDrawer is null!");
        }

        if (transform == null)
        {
            Debug.LogError("Transform is null!");
        }

        CreateGrid();
        gridDrawer.DrawGridLines(transform);

        allGroups = new List<List<BlockAbstract>>();
    }

    #region Grid Operations
    void CreateGrid()
    {
        grid = new Transform[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cell = new GameObject($"Cell ({x}, {y})");
                cell.transform.position = position;
                cell.transform.SetParent(transform);
                grid[x, y] = cell.transform;
            }
        }
    }
    public Vector3 GetCellPosition(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            return grid[x, y].position + new Vector3(cellSize / 2, cellSize / 2, 0);
        }
        return Vector2.zero;
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            return grid[x, y].childCount > 0;
        }
        return true;
    }

    public int FindLowestEmptyRow(int columnIndex)
    {
        Vector3 startPosition = GetCellPosition(columnIndex, rows - 1) + Vector3.up * cellSize / 2;
        float raycastDistance = rows * cellSize;
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.down, raycastDistance, LayerMask.GetMask("Obstacle"));

        int startRow = 0;
        if (hit.collider != null)
        {
            startRow = rows - 1 - Mathf.FloorToInt((startPosition.y - hit.point.y) / cellSize);
        }

        for (int row = startRow; row < rows; row++)
        {
            if (!IsCellOccupied(columnIndex, row))
            {
                return row;
            }
        }

        return rows - 1;
    }

    public void PlaceBlockInCell(int x, int y, GameObject block)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            block.transform.SetParent(grid[x, y], true);
        }
        else
        {
            Debug.LogError($"Cannot place block at invalid grid position ({x}, {y})");
        }
    }

    public BlockAbstract GetBlockAt(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            return grid[x, y].GetComponentInChildren<BlockAbstract>();
        }
        return null;
    }

    public Transform GetCellTransform(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            return grid[x, y];
        }
        return null;
    }
    #endregion

    #region Block Click Methods
    public void OnBlockClicked(BlockAbstract clickedBlock)
    {
        Debug.Log($"Block clicked: {clickedBlock.gameObject.name}");
        groupMaker.OnBlockClicked(clickedBlock);
        StartCoroutine(HandleBlockClickedCoroutine());
    }

    private IEnumerator HandleBlockClickedCoroutine()
    {
        _blockSpawnManager.OnGroupDestroyed();

        while (_blockSpawnManager.FallingBlocksCount > 0)
        {
            yield return null;
        }

        _blockSpawnManager.StartSpawnBlocksCoroutine();

        yield return new WaitForSeconds(0.1f);

        groupMaker.ValidateAndUpdateGroups();
    }
    #endregion

    #region Public GroupMaker Methods
    public void FindAllGroups()
    {
        groupMaker.FindAllGroups();
    }

    public void UpdateGroupSprites()
    {
        groupMaker.UpdateGroupSprites();
    }

    public void ValidateAndUpdateGroups()
    {
        groupMaker.ValidateAndUpdateGroups();
    }
    #endregion
}