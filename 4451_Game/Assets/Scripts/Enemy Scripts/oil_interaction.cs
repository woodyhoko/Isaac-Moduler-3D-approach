using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oil_interaction : MonoBehaviour
{
    player player;
    bool isTouchingPlayer = false;
    public int damage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<player>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<player>() != null)
        {
            isTouchingPlayer = true;
        }
    }

    private void Update()
    {
        if (isTouchingPlayer)
        {
            //Debug.Log("oil is hitting player");
            player.AlterHealth(-1 * damage);
        }
    }
}
