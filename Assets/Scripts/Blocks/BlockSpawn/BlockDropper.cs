using UnityEngine;

public class BlockDropper
{
    private GridManager gridManager;
    private float fallDuration;
    private BlockSpawnManager blockSpawnManager;

    public BlockDropper(BlockSpawnManager blockSpawnManager, GridManager gridManager, float fallDuration)
    {
        this.blockSpawnManager = blockSpawnManager;
        this.gridManager = gridManager;
        this.fallDuration = fallDuration;
    }

    public void DropBlocksToEmptyCells()
    {
        for (int column = 0; column < gridManager.Columns; column++)
        {
            for (int row = 0; row < gridManager.Rows; row++)
            {
                if (!gridManager.IsCellOccupied(column, row))
                {
                    for (int upperRow = row + 1; upperRow < gridManager.Rows; upperRow++)
                    {
                        BlockAbstract upperBlock = gridManager.GetBlockAt(column, upperRow);
                        if (upperBlock != null)
                        {
                            blockSpawnManager.IncreaseFallingBlocksCount();
                            gridManager.PlaceBlockInCell(column, row, upperBlock.gameObject);
                            //upperBlock.StartFalling(fallDuration, blockSpawnManager.DecreaseFallingBlocksCount);
                            upperBlock.StartFalling(fallDuration, () =>
                            {
                                blockSpawnManager.DecreaseFallingBlocksCount();
                                CheckAndDamageAdjacentObstacles(column, row);
                            });
                            break;
                        }
                    }
                }
            }
        }

        if (blockSpawnManager.FallingBlocksCount == 0)
        {
            blockSpawnManager.StartSpawnBlocksCoroutine();
        }
    }

    private void CheckAndDamageAdjacentObstacles(int column, int row)
    {
        CheckAndDamageObstacle(column + 1, row);
        CheckAndDamageObstacle(column - 1, row);
        CheckAndDamageObstacle(column, row + 1);
        CheckAndDamageObstacle(column, row - 1);
    }

    private void CheckAndDamageObstacle(int column, int row)
    {
        Transform cellTransform = gridManager.GetCellTransform(column, row);
        if (cellTransform != null)
        {
            Debug.Log($"Cell found at column: {column}, row: {row}");
            IDamageable obstacle = cellTransform.GetComponentInChildren<IDamageable>();
            obstacle?.TakeDamage();
        }
    }
}