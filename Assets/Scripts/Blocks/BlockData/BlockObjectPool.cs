using System.Collections.Generic;
using UnityEngine;

public class BlockObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public Queue<BlockAbstract> pooledBlocks;
        public GameObject blockPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools;

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var pool in pools)
        {
            pool.pooledBlocks = new Queue<BlockAbstract>();
            for (int i = 0; i < pool.poolSize; i++)
            {
                BlockAbstract block = CreateNewBlock(pool);
                pool.pooledBlocks.Enqueue(block);
            }
        }
    }

    private BlockAbstract CreateNewBlock(Pool pool)
    {
        GameObject blockObj = Instantiate(pool.blockPrefab);
        BlockAbstract block = blockObj.GetComponent<BlockAbstract>();
        blockObj.SetActive(false);
        return block;
    }

    public BlockAbstract GetBlock()
    {
        foreach (var pool in pools)
        {
            if (pool.pooledBlocks.Count > 0)
            {
                BlockAbstract block = pool.pooledBlocks.Dequeue();
                block.gameObject.SetActive(true);
                return block;
            }
        }
        return null;
    }

    public BlockAbstract GetBlockOfType(BlockAbstract blockType)
    {
        foreach (var pool in pools)
        {
            if (pool.blockPrefab.GetComponent<BlockAbstract>().GetType() == blockType.GetType() && pool.pooledBlocks.Count > 0)
            {
                BlockAbstract block = pool.pooledBlocks.Dequeue();
                block.gameObject.SetActive(true);
                return block;
            }
        }
        return null;
    }

    public void ReturnBlock(BlockAbstract block)
    {
        block.gameObject.SetActive(false);
        foreach (var pool in pools)
        {
            if (pool.blockPrefab == block.gameObject)
            {
                pool.pooledBlocks.Enqueue(block);
                return;
            }
        }
    }
}