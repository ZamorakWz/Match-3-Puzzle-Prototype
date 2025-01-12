using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "BlockData")]
public class BlockDataSO : ScriptableObject
{
    public Sprite defaultSprite;
    public Sprite firstConditionSprite;
    public Sprite secondConditionSprite;
    public Sprite thirdConditionSprite;
}