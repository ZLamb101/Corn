using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Damageable damageable;

    public ItemPickup[] itemDrops;

    public int xpValue = 30;

    public bool CanMove
    { 
        get 
        { 
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }



    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    public virtual void OnDeath()
    {
        ItemDrop();
        damageable.damageableDeath.RemoveListener(OnDeath);
        LevelingManager.Instance.GainExperienceFlatRate(xpValue);
    }

    private void ItemDrop() 
    {
        foreach(ItemPickup item in itemDrops)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}
