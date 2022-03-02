using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class starting_spawns : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            Debug.Log("am in level 2");
            transform.position = new Vector3(-72.6f, 22.73f, -27.45f);
        }
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            transform.position = new Vector3(-56.4f, -36.07f, -218.18f);
        }
    }

}
