using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_interactions : MonoBehaviour {

    private List<Vector3> roomCenters = new List<Vector3>();

    void Start()
    {
        roomCenters = getRoomCenters();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.name == "Door" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Hit something like a door ");
            this.transform.position = teleportPlayer(roomCenters);
            //SendMessageUpwards(levelUp);
        }

        if (hit.collider.tag == "enemyProjectile")
        {
            Debug.Log("Got hit with something like an enemy projectile");
        }
    }

    List<Vector3> getRoomCenters() {
        List<Vector3> returnLocations = new List<Vector3>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("floor"))
        {
            if (!this.GetComponent<Collider>().isTrigger)
            { 
                returnLocations.Add(g.transform.position);
            }
        }
        return returnLocations;
    }

    Vector3 teleportPlayer(List<Vector3> possibleLocations)
    {
        int randomIndex = Random.Range(0, possibleLocations.Count - 1);
        Debug.Log("teleports to: " + possibleLocations[randomIndex]);
        return possibleLocations[randomIndex];
    } 
}
