using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float health;

    [SerializeField]
    public AudioSource MusicSourcehit;
    [SerializeField]
    public AudioSource MusicSourcedie;

    private void Start()
    {
        if(name.Contains("crab"))
        {
            health = 7;
        }
        if(name.Contains("panemy"))
        {
            health = 5;
        }
        if(name.Contains("Boss"))
        {
            health = 50;
        }
    }

    private void Update()
    {
        if(health <= 0)
        {
            if(name.Contains("Boss"))
            {
                SendMessageUpwards("EndGame", true);
            }
            else
            {
                MusicSourcedie.Play();
                SendMessage("KillEnemy");
            }
            
        }
    }

    public void AdjustEnemyHealth(float amount)
    {
        MusicSourcehit.Play();
        health += amount;
    }
}
