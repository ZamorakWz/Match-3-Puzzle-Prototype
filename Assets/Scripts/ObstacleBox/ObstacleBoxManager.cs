using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBoxManager : MonoBehaviour, IDamageable
{
    [SerializeField] private Sprite fullHealthSprite;
    [SerializeField] private Sprite halfHealthSprite;

    private SpriteRenderer spriteRenderer;

    private int health = 2;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = health == 2 ? fullHealthSprite : halfHealthSprite;
    }
}