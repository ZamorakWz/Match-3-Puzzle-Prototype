using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    void Start()
    {
        CenterCamera();
    }

    private void CenterCamera()
    {
        float gridWidth = gridManager.Columns * gridManager.CellSize;
        float gridHeight = gridManager.Rows * gridManager.CellSize;

        Vector2 gridCenter = new Vector2(gridWidth / 2f, gridHeight / 2f);

        transform.position = new Vector3(gridCenter.x, gridCenter.y, transform.position.z);
    }
}