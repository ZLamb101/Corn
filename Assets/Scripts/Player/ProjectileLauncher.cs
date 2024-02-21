using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform LaunchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, LaunchPoint.position, projectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;

        // Flip the projectile's facing direction and movement bsaed on the direction the character is facing at time of Launch
        projectile.transform.localScale = new Vector3(
           origScale.x * transform.localScale.x > 0  ? 3 : -3, origScale.y, origScale.z );
    }
}
