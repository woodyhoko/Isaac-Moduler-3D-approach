using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{ 
    public GameObject[] spawnItem;
    private int amount;

    public GameController gamecontrol;

    private GameObject floor;
    floor fl;
    private HashSet<Vector3> unavailablePositions;
    
    static int spawnTries = 5;

    //debug vars
    private int enemySpawnCount;

    private void Start()
    {
        //enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        floor = GameObject.FindGameObjectWithTag("floor");
        if (floor != null)
        {
            fl = floor.GetComponent<floor>();
            unavailablePositions = fl.getTakenPositions();
        }
        doTheSpawn();
        //amount = 20;
        amount = gamecontrol.level * 5 + 4;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    foreach (GameObject g in spawnItem)
        //    {
        //        spawn(g);
        //    }
        checkLevelAdjustEnemyAmount();
    }

    void doTheSpawn()
    {
        foreach (GameObject g in spawnItem)
        {
            spawn(g);
        }
    }

    public bool existingEnemies()   //returns true if there exists enemies
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length > 0;
    }

    private void spawn(GameObject item)
    {
        Debug.Log("spawning stuff");
        int numTimes = item.tag == "door" ? 1 : amount;
        
        for (int i = 0; i < numTimes; i++)
        {
            Debug.Log("floor size x: " + floor.GetComponent<MeshRenderer>().bounds.size.x);
            Vector3 spawnLocation = findRandomGroundLocation(floor.GetComponent<MeshRenderer>().bounds.size.x, floor.GetComponent<MeshRenderer>().bounds.size.z);
            
            //if(unavailablePositions.Contains(spawnLocation))
            //{

            //}

            while (!isColliding(spawnLocation) && spawnTries > 0) //if is touching stuff other than floor find another loc
            {
                spawnLocation = findRandomGroundLocation(floor.GetComponent<MeshRenderer>().bounds.size.x, floor.GetComponent<MeshRenderer>().bounds.size.z);
                spawnTries--;
            }
            //Debug.Log(spawnLocation);
            if(spawnTries > 0 && isColliding(spawnLocation))
            {
                GameObject g = Instantiate(item) as GameObject;
                g.transform.position = spawnLocation;
                g.transform.parent = this.transform;
                spawnTries = 5;
            }
            //Debug.Log("item location: " + g.transform.position);
            //g.transform.position = spawnLocation;
        }
    }

    void checkLevelAdjustEnemyAmount()
    {
        switch (gamecontrol.level)
        {
            case 1:
                {
                    amount = 5;
                    break;
                }
            case 2:
                {
                    amount = 10;
                    break;
                }
        }
    }

    private bool isColliding(Vector3 spawnLoc)      //returns true if touching only floor
    {
        Collider[] hitColliders = Physics.OverlapSphere(spawnLoc, 2);
        Debug.Log("enemy hit this many things" + hitColliders.Length);
        return hitColliders.Length > 1;
    }

    private Vector3 findRandomGroundLocation(float x, float y)
    {
        float newX = Random.Range(-x/2, x/2);
        float newY = Random.Range(-y/2, y/2);
        //Debug.Log("New item random location x: " + newX + ", newY: " + newY + ".");
        return new Vector3(newX, 0, newY);
    }
}
