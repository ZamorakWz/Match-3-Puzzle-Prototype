using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleBoxPlacer : MonoBehaviour
{
    [SerializeField] private GameObject obstacleBoxPrefab;

    [SerializeField] private GridManager gridManager;

    void Start()
    {
        PlaceObstacleBox(0, 1);
        PlaceObstacleBox(1, 1);
        PlaceObstacleBox(2, 1);
        PlaceObstacleBox(3, 1);
        PlaceObstacleBox(4, 1);
    }

    //X variable refers to rows, and y refers to columns. Both of them start from 0
    public void PlaceObstacleBox(int x, int y)
    {
        Vector3 position = gridManager.GetCellPosition(x, y);
        GameObject obstacleBox = Instantiate(obstacleBoxPrefab, position, Quaternion.identity);
        gridManager.PlaceBlockInCell(x, y, obstacleBox);
    }
}