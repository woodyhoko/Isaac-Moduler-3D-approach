using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour{

    private int maxHealth = 5;
    public Text currentHealthText;
    public Texture2D heartTexture;

    public Text bulletStats;

    [HideInInspector]public Cannon bulletRef;
    [HideInInspector]public float bulletnum;
    [HideInInspector]public float bulletdmg;

    public static int currentHealth;
    private Rect healthRectPos;

    GameController gameController;

    private void Start()
    {
        //transform.position.y = 0;
        currentHealth = 3;
        //Debug.Log(currentHealth);
        if (heartTexture == null) heartTexture = new Texture2D(1, 1);
        bulletRef = GetComponent<Cannon>();
        bulletdmg = bulletRef.returnCurrDamage();
        bulletnum = bulletRef.returnCurrBalls();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public void ChangePlayerLoc(Vector3 loc)
    {
        transform.position = loc;
    }

    void OnGUI()
    {
        int i = currentHealth;
        float spacing = 40;
        while(i > 0)
        {
            GUI.DrawTexture(new Rect(30 + spacing, 5, 30, 30), heartTexture);
            spacing += 35;
            i--;
        }
        if(currentHealthText != null)
        {
            currentHealthText.text = currentHealth.ToString();
        }
        if(bulletStats != null)
        {
            bulletStats.text = "ball num: " + bulletnum.ToString() + "bull dmg: " + bulletdmg.ToString();
        }
    }

    private void Update()
    {
        if(currentHealth == 0)
        {
            //SendMessageUpwards("EndGame");
            if(gameController != null)
            {
                gameController.EndGame(false);
            }
        }
        if(this.transform.position.y < -50)
        {
            if (gameController != null)
            {
                gameController.EndGame(false);
            } else
            {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
                gameController.EndGame(false);
            }
        }
        bulletdmg = bulletRef.returnCurrDamage();
        bulletnum = bulletRef.returnCurrBalls();
    }

    public void AlterHealth(int amount)
    {
        //Debug.Log("Current health before attack: " + currentHealth);
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log("Whoooo alter the player health this much: " + amount);
        //Debug.Log("Current health: " + currentHealth);
    }
}
