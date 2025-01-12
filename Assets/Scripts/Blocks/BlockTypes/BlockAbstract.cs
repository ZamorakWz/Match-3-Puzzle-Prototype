using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public abstract class BlockAbstract : MonoBehaviour, IDamageable
{
    private Action OnFallCompleteCallBack;
    [SerializeField] private BlockDataSO blockData;

    private SpriteRenderer SpriteRenderer;
    private GridManager gridManager;
    protected BlockObjectPool blockObjectPool;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        blockObjectPool = FindObjectOfType<BlockObjectPool>();
        gridManager = FindObjectOfType<GridManager>();
    }

    private void OnEnable()
    {
        UpdateSprite(SpriteCondition.Default);
    }

    public void UpdateSprite(SpriteCondition condition)
    {
        if (SpriteRenderer == null) return;
        switch (condition)
        {
            case SpriteCondition.First:
                SpriteRenderer.sprite = blockData.firstConditionSprite;
                break;
            case SpriteCondition.Second:
                SpriteRenderer.sprite = blockData.secondConditionSprite;
                break;
            case SpriteCondition.Third:
                SpriteRenderer.sprite = blockData.thirdConditionSprite;
                break;
            default:
                SpriteRenderer.sprite = blockData.defaultSprite;
                break;
        }
    }
    public enum SpriteCondition { Default, First, Second, Third }

    public string GetBlockTag()
    {
        return tag;
    }

    public void StartFalling(float fallDuration, Action onFallComplete)
    {
        OnFallCompleteCallBack = onFallComplete;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = transform.parent.position + new Vector3(gridManager.CellSize / 2, gridManager.CellSize / 2, 0);
        transform.DOMove(targetPosition, fallDuration).SetEase(Ease.OutBounce).OnComplete(OnFallComplete);
    }

    private void OnFallComplete()
    {
        if (gameObject == null) return;
        OnFallCompleteCallBack?.Invoke();
    }

    public void TakeDamage()
    {
        Debug.Log($"Returning block: {gameObject.name} to pool");
        blockObjectPool.ReturnBlock(this);
        transform.SetParent(null);
    }
}