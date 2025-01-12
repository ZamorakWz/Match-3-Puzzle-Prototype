using System.Collections.Generic;
using System.Diagnostics;

public class GroupMaker
{
    private GridManager gridManager;
    private List<List<BlockAbstract>> allGroups;

    public GroupMaker(GridManager gridManager)
    {
        this.gridManager = gridManager;
        allGroups = new List<List<BlockAbstract>>();
    }

    public void FindAllGroups()
    {
        bool[,] visited = new bool[gridManager.Columns, gridManager.Rows];

        for (int x = 0; x < gridManager.Columns; x++)
        {
            for (int y = 0; y < gridManager.Rows; y++)
            {
                BlockAbstract block = gridManager.GetBlockAt(x, y);
                if (block != null && !visited[x, y])
                {
                    List<BlockAbstract> group = new List<BlockAbstract>();
                    ApplyDFSAlgorithm(x, y, block.GetBlockTag(), group, visited);

                    if (group.Count > 1)
                    {
                        allGroups.Add(group);
                        Debug.WriteLine($"Group found with {group.Count} blocks.");
                    }
                }
            }
        }
    }

    private void ApplyDFSAlgorithm(int x, int y, string targetTag, List<BlockAbstract> group, bool[,] visited)
    {
        if (x < 0 || x >= gridManager.Columns || y < 0 || y >= gridManager.Rows || visited[x, y])
            return;

        BlockAbstract currentBlock = gridManager.GetBlockAt(x, y);
        if (currentBlock == null || currentBlock.GetBlockTag() != targetTag)
            return;

        visited[x, y] = true;
        group.Add(currentBlock);

        ApplyDFSAlgorithm(x + 1, y, targetTag, group, visited);
        ApplyDFSAlgorithm(x - 1, y, targetTag, group, visited);
        ApplyDFSAlgorithm(x, y + 1, targetTag, group, visited);
        ApplyDFSAlgorithm(x, y - 1, targetTag, group, visited);
    }

    public void UpdateGroupSprites()
    {
        foreach (var group in allGroups)
        {
            BlockAbstract.SpriteCondition condition;

            switch (group.Count)
            {
                case >= 9:
                    condition = BlockAbstract.SpriteCondition.Third;
                    break;
                case >= 5:
                    condition = BlockAbstract.SpriteCondition.Second;
                    break;
                case >= 2:
                    condition = BlockAbstract.SpriteCondition.First;
                    break;
                default:
                    condition = BlockAbstract.SpriteCondition.Default;
                    break;
            }

            foreach (BlockAbstract block in group)
            {
                if (block != null)
                {
                    block.UpdateSprite(condition);
                }
            }
        }
    }

    public void OnBlockClicked(BlockAbstract clickedBlock)
    {
        List<BlockAbstract> groupToDestroy = null;

        foreach (var group in allGroups)
        {
            if (group.Contains(clickedBlock))
            {
                groupToDestroy = group;
                break;
            }
        }

        if (groupToDestroy != null)
        {
            Debug.WriteLine($"Destroying group with {groupToDestroy.Count} blocks");
            foreach (var block in groupToDestroy)
            {
                block.TakeDamage();
            }

            allGroups.Remove(groupToDestroy);
        }
        else
        {
            Debug.WriteLine("No group found for the clicked block");
        }
    }

    public void ClearAllGroups()
    {
        if (allGroups.Count != 0)
        {
            allGroups.Clear();
        }
    }

    public void ValidateAndUpdateGroups()
    {
        ClearAllGroups();
        FindAllGroups();
        UpdateGroupSprites();
    }
}