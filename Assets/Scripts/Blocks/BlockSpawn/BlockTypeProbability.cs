using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockTypeProbability
{
    public BlockAbstract blockType;
    [Range(0f, 100f)]
    public float probability;
}