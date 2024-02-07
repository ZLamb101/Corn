using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D selectedObject;
    private bool isFadingOut = false;
    private float fadeTime = 1.0f;
    private float fadeTimer = 0.0f;
    private float riseSpeed = 1.5f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!isFadingOut && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                selectedObject = hit.collider.GetComponent<Rigidbody2D>();
                isFadingOut = true;
            }
        }

        if (isFadingOut)
        {
            FadeOutObject();
        }
    }

    void FadeOutObject()
    {
        fadeTimer += Time.deltaTime;
        float alpha = 1.0f - (fadeTimer / fadeTime);
        Color objectColor = selectedObject.GetComponent<SpriteRenderer>().color;
        objectColor.a = alpha;
        selectedObject.GetComponent<SpriteRenderer>().color = objectColor;

        // Rising effect
        Vector2 newPosition = selectedObject.transform.position;
        newPosition.y += riseSpeed * Time.deltaTime;
        selectedObject.MovePosition(newPosition);

        if (fadeTimer >= fadeTime)
        {
            Destroy(selectedObject.gameObject);
            isFadingOut = false;
            selectedObject = null;
            fadeTimer = 0.0f;
        }
    }
}

