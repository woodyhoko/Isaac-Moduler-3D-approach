using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    player player;

    //[SerializeField]
    //public AudioSource MusicSourceupgrade;

    public Text pickUp;
    GameObject canvas;
    private string thingPickedUp;
    private bool colddown = true;

    [SerializeField]
    public AudioSource MusicSourceupgrade;

    private void Start()
    {
        player = GameObject.FindWithTag("player").GetComponent<player>();
        canvas = GameObject.FindWithTag("canvas");
        pickUp.text = "";
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (colddown && other.GetComponent<player>() != null) //other is the player
        {
            MusicSourceupgrade.Play();
            colddown = false;
            //Debug.Log("player has hit me_e");
            if (tag == "health_apple")
            {
                
                player.AlterHealth(1);
                thingPickedUp = "apple - health up";
                //Debug.Log("player health up");
            }
            if(tag == "balls_glove")
            {
                player.GetComponent<Cannon>().increaseNumberofBalls();
                thingPickedUp = "gloves - balls up";
                //Debug.Log("player bullet up");
            }
            if (tag == "sticky_glue")
            {
                player.GetComponent<Cannon>().toggleStickyMode();
                thingPickedUp = "stick of glue - sticky up";
                //player.bulletStats.text += "sticky mode: ON";
            }
            if (tag == "balls_mushroom")
            {
                player.GetComponent<Cannon>().increaseBallSize();
                thingPickedUp = "mushrooms - balls size up";
                //Debug.Log("player balls up");
            }
            if(tag == "homing_magneato_helmet")
            {
                player.GetComponent<Cannon>().toggleHomingMode();
                thingPickedUp = "magneto's helmet - homing on";
            }
            UpdatePickUpText(thingPickedUp);
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            colddown = true;
        }
    }

    void UpdatePickUpText(string thingPickedUp)
    {
        //MusicSourceupgrade.Play();
        pickUp = Instantiate(pickUp);
        canvas = GameObject.Find("Canvas");
        pickUp.text = thingPickedUp;
        if(canvas != null) 
            pickUp.transform.SetParent(canvas.transform, false);
        Destroy(pickUp.gameObject, 3);
    }
}
