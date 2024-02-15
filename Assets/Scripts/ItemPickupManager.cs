using UnityEngine;
using System.Collections.Generic;
using Inventory.Model;

public class ItemPickupManager : MonoBehaviour
{
    public Collider2D playerCollider;
    private Camera mainCamera;
    private List<Rigidbody2D> selectedObjects = new List<Rigidbody2D>();
    private bool isFadingOut = false;
    private float fadeTime = 0.3f;
    private float fadeTimer = 0.0f;
    private float riseSpeed = 1.5f;

    [SerializeField]
    private InventorySO inventoryData;

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemDrop item = other.GetComponent<ItemDrop>();
        if (item != null)
        {
            int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (remainder == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item.Quantity = remainder;
            }
        }
    }

    void Update()
    {
        if (!isFadingOut && Input.GetKey(KeyCode.E))
        {
            selectedObjects = GetDropsInPlayerHitbox();

            if (selectedObjects.Count > 0)
            {
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            FadeOutObjects();
        }
    }

    List<Rigidbody2D> GetDropsInPlayerHitbox()
    {
        List<Rigidbody2D> newSelectedObjects = new List<Rigidbody2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Drops"));
        List<Collider2D> colliders = new List<Collider2D>(); // Use List instead of array
        playerCollider.OverlapCollider(filter, colliders);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D obj = collider.GetComponent<Rigidbody2D>();
            if (obj != null)
            {

                newSelectedObjects.Add(obj);
            }
        }
        return newSelectedObjects;
    }

    void FadeOutObjects()
    {
        fadeTimer += Time.deltaTime;
        float alpha = 1.0f - (fadeTimer / fadeTime);

        foreach (Rigidbody2D selectedObject in selectedObjects)
        {
            SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
            Color objectColor = spriteRenderer.color;
            objectColor.a = alpha;
            spriteRenderer.color = objectColor;

            // Rising effect
            Vector2 newPosition = selectedObject.transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            selectedObject.MovePosition(newPosition);
        }

        if (fadeTimer >= fadeTime)
        {
            foreach (Rigidbody2D selectedObject in selectedObjects)
            {
                ItemDrop item = selectedObject.GetComponent<ItemDrop>();
                if (item != null)
                {
                    int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                    if (remainder == 0)
                    {
                        Debug.Log("Destroying item");
                        item.DestroyItem();
                    }
                    else
                    {
                        Debug.Log("Remainder: " + remainder);
                        item.Quantity = remainder;

                        SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
                        Color objectColor = spriteRenderer.color;
                        objectColor.a = 1.0f;
                        spriteRenderer.color = objectColor;
                    }
                }
            }
            selectedObjects.Clear();
            isFadingOut = false;
            fadeTimer = 0.0f;
        }
    }

   
}