using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyManager : MonoBehaviour
{

    public static Dictionary<string, List<Vector2>> enemyToRespawn;

    private void Awake()
    {
        enemyToRespawn = new Dictionary<string, List<Vector2>>
            {
                {"FoxEnemy", new List<Vector2>()},
                {"FlyingEye" , new List<Vector2>()}
        };
    }


    private void Update()
    {
        // Check each dictionary value for respawn
        // Respawn(Dictionary)
        foreach(var enemy in enemyToRespawn) {
            var type = enemy.Key;
            EnemyRespawn(type);
        }
       ;
    }



    public void EnemyRespawn(string enemyType)
    {
        var list = enemyToRespawn[enemyType];
        foreach (var position in list)
        {
            // TODO find a way to delay spawns
            //Invoke("RespawnDelay", 5);

            GameObject enemyClone = Instantiate((GameObject)Resources.Load(enemyType), new Vector3(position.x, position.y, 0), Quaternion.identity);
            enemyClone.transform.parent = GameObject.Find("EnemyManager").transform;
            if (enemyType == "FlyingEye")
            {
                // TODO Find a way to assign waypoints
                enemyClone.transform.position = new Vector3(position.x, position.y + 3, 0);
            }

            Debug.Log("remove one from list"); 
        }
        list.Clear();
        // Create text at character hit
    }

    public void RespawnDelay(string enemyType, Vector2 position)
    {
        GameObject enemyClone = Instantiate((GameObject)Resources.Load(enemyType), new Vector3(position.x, position.y, 0), Quaternion.identity);
        enemyClone.transform.parent = GameObject.Find("EnemyManager").transform;
        if (enemyType == "FlyingEye")
        {
            //Find a way to assign waypoints
            enemyClone.transform.position = new Vector3(position.x, position.y + 3, 0);
        }
    }
}
