using UnityEngine;
using System.Collections.Generic;

public class ItemPickupManager : MonoBehaviour
{
    public Collider2D playerCollider;
    private Camera mainCamera;
    private List<Rigidbody2D> selectedObjects = new List<Rigidbody2D>();
    private bool isFadingOut = false;
    private float fadeTime = 0.3f;
    private float fadeTimer = 0.0f;
    private float riseSpeed = 1.5f;

    void Start()
    {
        mainCamera = Camera.main;
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

    /*   List<Rigidbody2D> GetObjectsUnderCursor()
       {
           List<Rigidbody2D> newSelectedObjects = new List<Rigidbody2D>();
           RaycastHit2D[] hits = Physics2D.RaycastAll(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
           foreach (RaycastHit2D hit in hits)
           {
               if (hit.collider != null && hit.collider.GetComponent<Rigidbody2D>() != null && hit.collider.CompareTag("Drops"))
               {
                   Rigidbody2D obj = hit.collider.GetComponent<Rigidbody2D>();
                   newSelectedObjects.Add(obj);
               }
           }
           return newSelectedObjects;
       }*/

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
            Color objectColor = selectedObject.GetComponent<SpriteRenderer>().color;
            objectColor.a = alpha;
            selectedObject.GetComponent<SpriteRenderer>().color = objectColor;

            // Rising effect
            Vector2 newPosition = selectedObject.transform.position;
            newPosition.y += riseSpeed * Time.deltaTime;
            selectedObject.MovePosition(newPosition);
        }

        if (fadeTimer >= fadeTime)
        {
            foreach (Rigidbody2D selectedObject in selectedObjects)
            {
                Destroy(selectedObject.gameObject);
            }
            selectedObjects.Clear();
            isFadingOut = false;
            fadeTimer = 0.0f;
        }
    }
}
