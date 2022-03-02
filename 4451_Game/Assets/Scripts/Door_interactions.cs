using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_interactions : MonoBehaviour
{

    [SerializeField]
    public AudioSource music;
    void OnTriggerEnter(Collider other)
    {
       if(other.tag == "player")
        {
            music.mute = true;
            Debug.Log("sendin msg level up");
            SendMessageUpwards("levelUp");
        }
    }
}