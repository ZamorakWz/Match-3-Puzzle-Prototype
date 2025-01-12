using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlockSpawner
{
    private BlockSpawnManager blockSpawnManager;
    private GridManager gridManager;
    private BlockObjectPool blockPool;
    private float blocksAboveGrid;
    private float fallDuration;
    private List<BlockTypeProbability> blockProbabilities;

    public BlockSpawner(BlockSpawnManager blockSpawnManager, GridManager gridManager, BlockObjectPool blockPool, float blocksAboveGrid, float fallDuration, List<BlockTypeProbability> blockProbabilities)
    {
        this.blockSpawnManager = blockSpawnManager;
        this.gridManager = gridManager;
        this.blockPool = blockPool;
        this.blocksAboveGrid = blocksAboveGrid;
        this.fallDuration = fallDuration;
        this.blockProbabilities = blockProbabilities;
    }

    public IEnumerator SpawnBlocksCoroutine()
    {
        bool allColumnsFilled = false;

        while (!allColumnsFilled)
        {
            allColumnsFilled = true;
            for (int column = 0; column < gridManager.Columns; column++)
            {
                int targetRow = gridManager.FindLowestEmptyRow(column);
                Debug.Log($"Column: {column}, Lowest Empty Row: {targetRow}");

                if (targetRow != -1 && !gridManager.IsCellOccupied(column, targetRow))
                {
                    BlockAbstract block = GetRandomBlock();
                    if (block != null)
                    {
                        try
                        {
                            gridManager.PlaceBlockInCell(column, targetRow, block.gameObject);
                            Vector3 startPos = gridManager.GetCellPosition(column, gridManager.Rows - 1)
                                               + Vector3.up * (blocksAboveGrid * gridManager.CellSize);
                            block.transform.position = startPos;
                            blockSpawnManager.IncreaseFallingBlocksCount();
                            block.StartFalling(fallDuration, blockSpawnManager.DecreaseFallingBlocksCount);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"Error spawning block: {e.Message}, {e.StackTrace}");
                        }


                    }
                    allColumnsFilled = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private BlockAbstract GetRandomBlock()
    {
        float randomValue = Random.Range(0f, 100f);
        float cumulative = 0f;
        foreach (var blockType in blockProbabilities)
        {
            cumulative += blockType.probability;
            if (randomValue <= cumulative)
            {
                return blockPool.GetBlockOfType(blockType.blockType);
            }
        }
        return blockPool.GetBlockOfType(blockProbabilities[blockProbabilities.Count - 1].blockType);
    }
}