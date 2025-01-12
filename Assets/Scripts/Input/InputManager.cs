using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private BlockSpawnManager _blockSpawnManager;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectBlockClick();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _blockSpawnManager.StartSpawnBlocksCoroutine();
        }
    }

    private void DetectBlockClick()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            BlockAbstract clickedBlock = hit.collider.GetComponent<BlockAbstract>();

            if (clickedBlock != null)
            {
                _gridManager.OnBlockClicked(clickedBlock);
            }
        }
    }
}