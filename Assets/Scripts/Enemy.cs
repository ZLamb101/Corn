using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    public static int timesRespawned =0 ;

    public float moveSpeed = 1.5f;

    protected UnityEngine.Object enemyRef;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Damageable damageable;

    public float delayOnRespawn;

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
        Invoke("Respawn", 1);
        
        //StartCoroutine("Respawn");
        Debug.Log("hello world3");
    }

    private void Respawn()
    {
        timesRespawned++;
        Debug.Log(timesRespawned);

        //Add to the queue for enemy Manager to pickup
        EnemyManager.enemyToRespawn[enemyRef.name].Add(new Vector2(transform.position.x, transform.position.y));
    }

    private void ItemDrop() 
    {
        foreach(ItemPickup item in itemDrops)
        {
           var newObject = Instantiate(item, transform.position, Quaternion.identity);
            newObject.transform.parent = GameObject.Find("LootManager").transform;
        }
    }
}
