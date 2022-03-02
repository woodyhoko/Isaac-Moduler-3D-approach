using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorgeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject floorobject;
    [SerializeField]
    private GameObject walls;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool fr = true;
        bool fl = true;
        bool br = true;
        bool bl = true;
        bool fm = true;
        bool bm = true;
        bool rm = true;
        bool lm = true;

        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("floor")) {
            if (fr && floor.transform.position[0]-25 <= this.transform.position[0]+70 && floor.transform.position[0] + 25 >= this.transform.position[0] + 70 && floor.transform.position[2] - 25 <= this.transform.position[2] + 70 && floor.transform.position[2] + 25 >= this.transform.position[2] + 70)
            {
                fr = false;
            }
            else if (fl && floor.transform.position[0] - 25 <= this.transform.position[0] + 70 && floor.transform.position[0] + 25 >= this.transform.position[0] + 70 && floor.transform.position[2] - 25 <= this.transform.position[2] - 70 && floor.transform.position[2] + 25 >= this.transform.position[2] - 70)
            {
                fl = false;
            }
            else if (br && floor.transform.position[0] - 25 <= this.transform.position[0] - 70 && floor.transform.position[0] + 25 >= this.transform.position[0] - 70 && floor.transform.position[2] - 25 <= this.transform.position[2] + 70 && floor.transform.position[2] + 25 >= this.transform.position[2] + 70)
            {
                br = false;
            }
            else if (bl && floor.transform.position[0] - 25 <= this.transform.position[0] - 70 && floor.transform.position[0] + 25 >= this.transform.position[0] - 70 && floor.transform.position[2] - 25 <= this.transform.position[2] - 70 && floor.transform.position[2] + 25 >= this.transform.position[2] - 70)
            {
                bl = false;
            }
            else if (fm && floor.transform.position[0] - 25 <= this.transform.position[0] + 70 && floor.transform.position[0] + 25 >= this.transform.position[0] + 70 && floor.transform.position[2] - 25 <= this.transform.position[2] && floor.transform.position[2] + 25 >= this.transform.position[2])
            {
                fm = false;
            }
            else if (bm && floor.transform.position[0] - 25 <= this.transform.position[0] - 70 && floor.transform.position[0] + 25 >= this.transform.position[0] - 70 && floor.transform.position[2] - 25 <= this.transform.position[2] && floor.transform.position[2] + 25 >= this.transform.position[2])
            {
                bm = false;
            }
            else if (rm && floor.transform.position[0] - 25 <= this.transform.position[0] && floor.transform.position[0] + 25 >= this.transform.position[0] && floor.transform.position[2] - 25 <= this.transform.position[2] + 70 && floor.transform.position[2] + 25 >= this.transform.position[2] + 70)
            {
                rm = false;
            }
            else if (lm && floor.transform.position[0] - 25 <= this.transform.position[0] && floor.transform.position[0] + 25 >= this.transform.position[0] && floor.transform.position[2] - 25 <= this.transform.position[2] - 70 && floor.transform.position[2] + 25 >= this.transform.position[2] - 70)
            {
                lm = false;
            }
        };
        if (fr){
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50, 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50 -25, 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (fl)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50, 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (br)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50, 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (bl)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50 , 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (fm)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50 , 0, ((int)(this.transform.position[2] + 25) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] + 94.9) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] + 25) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (bm)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50 , 0, ((int)(this.transform.position[2] + 25) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] - 94.9) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] + 25) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (rm)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] + 25) / 50) * 50 , 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 ), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] + 25) / 50) * 50 - 25, 0, ((int)(this.transform.position[2] + 94.9) / 50) * 50 - 25), new Quaternion(0, 0, 0, 0));
        }
        if (lm)
        {
            Instantiate(floorobject, new Vector3(((int)(this.transform.position[0] + 25) / 50) * 50 , 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50), new Quaternion(0, 0, 0, 0));
            Instantiate(walls, new Vector3(((int)(this.transform.position[0] + 25) / 50) * 50, 0, ((int)(this.transform.position[2] - 94.9) / 50) * 50), new Quaternion(0, 0, 0, 0));
        }
    }
}
