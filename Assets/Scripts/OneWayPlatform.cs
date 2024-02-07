using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    public float waitTime;
    private bool _playerOnPlatform;

    private void Awake()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitTime = 0.5f;
    }

    private void Update()
    {
        if(_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            effector.useColliderMask = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(waitTime);
        effector.useColliderMask = true;
    }

    // Update is called once per frame
    public void DisableCollision(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            _playerOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DisableCollision(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        DisableCollision(collision, false);
    }
}
