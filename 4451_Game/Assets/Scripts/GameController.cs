using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text gameOverText;
    public Text scoreText;
    //public Text levelText;
    public GameObject DontDestroy;
    //public GameObject restart;

    player player;
    private int score;
    private bool gameOver;
    private bool enemiesCleared;

    public int level;

    private void Start()
    {
        gameOver = false;
        score = 0;
        level = 1;
        player = GameObject.FindWithTag("player").GetComponent<player>();
    }

    private void Update()
    {
        if(gameOverText == null)
        {
            gameOverText = GameObject.FindGameObjectWithTag("go_text").GetComponent<Text>();
        }
        if (scoreText == null)
        {
            scoreText = GameObject.FindGameObjectWithTag("score_text").GetComponent<Text>();
        }
        //if (levelText == null)
        //{
        //    levelText = GameObject.FindGameObjectWithTag("level_text").GetComponent<Text>();
        //}
        if (DontDestroy == null)
        {
            DontDestroy = GameObject.FindGameObjectWithTag("dontdestroy");
        }

        //if(restart == null)
        //{
        //    restart = GameObject.Find("Button");
        //}

        //if(gameOver)
        //{
        //    restart.GetComponent<Button>().onClick.AddListener(Restart);
        //}
        if(player.currentHealth <= 0)
        {
            EndGame(false);
        }
    }

    void AdjustScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void levelUp()
    {
        Debug.Log("levelUp called");
        if(!GetComponentInChildren<spawner>().existingEnemies())
        {
            Debug.Log("can actually level up");
            if(level == 0)
            {
                level = 1;
            } else
            {
                level++;
                DontDestroyOnLoad(DontDestroy);
                if (SceneManager.GetActiveScene().name == "Level1")
                {
                    player.ChangePlayerLoc(new Vector3(-72.6f, 17f, -11.84f));
                    SceneManager.LoadScene("Level2");
                }
                if (SceneManager.GetActiveScene().name == "Level2")
                {
                    player.ChangePlayerLoc(new Vector3(-56.4f, -36.07f, -218.18f));
                    SceneManager.LoadScene("Level3");
                }
                if (SceneManager.GetActiveScene().name == "Level3")
                {
                    player.ChangePlayerLoc(new Vector3(24.43f, 232.8f, 24.54f));
                    SceneManager.LoadScene("boss");
                }
            }
        }
    }

    public void EndGame(bool win)
    {
        if(win)
        {
            gameOverText.text = "You beat the boss!! Game Over!";
        } else
        {
            gameOverText.text = "You have died. Game Over! You got to Level: " + level.ToString();
        }
       
        gameOver = true;
        Time.timeScale = 0;
        //Vector3 diePosition = player.transform.localPosition;
        player.GetComponent<Transform>().position = Vector3.zero;
        //restart.GetComponent<Button>().image.enabled = true;
        //restart.GetComponentInChildren<Text>().text = "Restart";
        //BroadcastMessage("stopPlayerMvmt");
        //player.GetComponent<CharacterController>().enabled = false;
    }

    //void Restart()
    //{
    //    restart.GetComponentInChildren<Text>().text = "";
    //    restart.GetComponent<Button>().image.enabled = false;
    //    Time.timeScale = 1;
    //    SceneManager.LoadScene("Level1");
    //}
}
