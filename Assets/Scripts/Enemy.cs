using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Damageable))]
public class Enemy : MonoBehaviour
{
    public static int timesRespawned =0 ;
    private Random rnd = new Random();

    public float moveSpeed = 1.5f;

    protected UnityEngine.Object enemyRef;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Damageable damageable;
    SpriteRenderer spriteRenderer;

    public float delayOnRespawn;

    public DropTable dropTable; 
    public int xpValue = 30;
    
    private WalkableDirection _walkDirection;
    protected Vector2 walkDirectionVector = Vector2.right;
    public enum WalkableDirection { Right, Left }

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                //Direction Flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool CanMove
    { 
        get 
        { 
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Invoke("Respawn", delayOnRespawn);
        gameObject.SetActive(false);
    }

    private void Respawn()
    {
        timesRespawned++;
        Debug.Log(timesRespawned);

        gameObject.SetActive(true);
        damageable.Health = damageable.MaxHealth;
        damageable.IsAlive = true;
        spriteRenderer.color = new Color(255,255,255,255);
        
        if (rnd.Next(2) == 1)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else
        {
            WalkDirection = WalkableDirection.Right;
        }
    }

    private void ItemDrop() 
    {
        if (dropTable != null)
        {
            foreach (var entry in dropTable.entries)
            {
                if (UnityEngine.Random.Range(0f, 1f) <= entry.dropRate)
                {
                    Instantiate(entry.item, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
