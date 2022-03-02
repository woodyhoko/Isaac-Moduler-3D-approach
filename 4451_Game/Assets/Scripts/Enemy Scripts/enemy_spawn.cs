using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_spawn : MonoBehaviour {

    public GameObject melee_Enemy;
    public int numberOfEnemies;

    private GameObject floor;
    private int spawnTries = 5;

    private void Start()
    {
        floor = GameObject.FindGameObjectWithTag("floor");
        while(numberOfEnemies > 0)
        {
            placeEnemy(melee_Enemy);
            checkSpawnEnemy(melee_Enemy);
            numberOfEnemies--;
        }
    }

    private void placeEnemy(GameObject enemy)
    {

    }

    private void checkSpawnEnemy(GameObject enemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, 2);
        //spawn at different location if there is a block where enemy spawned

        foreach(Collider c in hitColliders) {
            if(c.tag == "environment_block")
            {
                while(spawnTries > 0)
                {

                }
            }
        }
    }
}
