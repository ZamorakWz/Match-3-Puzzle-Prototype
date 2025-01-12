using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class BlockSpawnManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float blocksAboveGrid = 1.5f;
    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private List<BlockTypeProbability> blockProbabilities;
    [SerializeField] private int fallingBlocksCount = 0;
    public int FallingBlocksCount => fallingBlocksCount;

    private BlockSpawner blockSpawner;
    private BlockDropper blockDropper;

    private BlockObjectPool _blockPool;

    [Inject]
    public void Construct(BlockObjectPool blockPool)
    {
        _blockPool = blockPool;
    }
    private void Awake()
    {
        blockSpawner = new BlockSpawner(this, gridManager, _blockPool, blocksAboveGrid, fallDuration, blockProbabilities);
        blockDropper = new BlockDropper(this, gridManager, fallDuration);

        ValidateBlockSpawnProbabilities();
    }
    private void ValidateBlockSpawnProbabilities()
    {
        float total = 0f;
        foreach (var blockType in blockProbabilities)
        {
            total += blockType.probability;
        }
        if (Mathf.Abs(total - 100f) > 0.1f)
        {
            Debug.LogWarning($"Total block drop probability is not 100%! Current total: {total}");
        }
    }

    public void StartSpawnBlocksCoroutine()
    {
        StartCoroutine(blockSpawner.SpawnBlocksCoroutine());
    }
    public void OnGroupDestroyed()
    {
        blockDropper.DropBlocksToEmptyCells();
    }
    public void IncreaseFallingBlocksCount()
    {
        fallingBlocksCount++;
    }
    public void DecreaseFallingBlocksCount()
    {
        fallingBlocksCount--;
        if (FallingBlocksCount == 0)
        {
            OnAllBlocksDropped();
        }
    }
    private void OnAllBlocksDropped()
    {
        gridManager.FindAllGroups();
        gridManager.UpdateGroupSprites();
        StartSpawnBlocksCoroutine();
    }
}