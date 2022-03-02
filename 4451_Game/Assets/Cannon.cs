using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private Rigidbody cannonballInstance;
    private GameObject enemy;

    [SerializeField]
    [Range(10f, 80f)]
    public float angle = 45f;
    public int nn = 1;
    public float ss = 1;
    public bool homo_mode = false;
    public float homo_strength = 0.1f;
    private bool conti_shoot = false;
    private int recoll_speed = 10;
    private int current_recoll_colddown = 0;
    private int damage = 1;
    public bool shooting_mode = false;
    public bool bounce_mode = true;
    private bool float_ball = false;
    public bool stick_mode = false;
    public bool turret_mode = false;
    [SerializeField]
    public AudioClip MusicClip;
    [SerializeField]
    public AudioSource MusicSource;
    //cannon_ball bscript;

    private System.Collections.Generic.List<Rigidbody> cannonslist = new System.Collections.Generic.List<Rigidbody>();


    void Awake()
    {
        //cannonballInstance = (UnityEngine.Rigidbody)Resources.Load("Cannonball");
        //bscript = GameObject.Find("CannonBall").GetComponent<cannon_ball>();
        enemy = GameObject.Find("Cube");
        MusicSource.clip = MusicClip;
        //Debug.Log("Editor causes this Awake");
    }

    //void DestroyObjectDelayed()
    //{
    //    // Kills the game object in 5 seconds after loading the object
    //    Destroy(cannonballInstance, 5);
    //}



    private void Update()
    {
        for (int n = 0; n < cannonslist.Count; n++)
        {
            if (cannonslist[n] == null)
            {
                cannonslist.RemoveAt(n);
                if (n == 0)
                {
                    break;
                }
                n--;
            }
        }

        if (homo_mode)
        {
            for (int n = 0; n < cannonslist.Count; n++)
            {
                if (cannonslist[n] == null)
                {
                    cannonslist.RemoveAt(n);
                    if (n == 0)
                    {
                        break;
                    }
                    n--;
                }
                if(enemy != null) 
                    cannonslist[n].velocity += (enemy.transform.position - cannonslist[n].transform.position).normalized * homo_strength;
            }
        }

        if (current_recoll_colddown-- <0 && (conti_shoot || Input.GetMouseButtonDown(0)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {

                float delay_time = 0.1f;
                if (shooting_mode)
                {
                    delay_time = 0.1f;
                }
                for (int n = 0; n < nn; n++)
                {
                    MusicSource.Play();
                    StartCoroutine(FireCannonAtPoint(hitInfo.point, n * delay_time));
                }
            }
            current_recoll_colddown = recoll_speed;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            float_ball = !float_ball;
        }
        //if (Input.GetKey(KeyCode.L))
        //{
        //    ss *= 1.1f;
        //}
        if (Input.GetKey(KeyCode.K))
        {
            ss *= 0.9f;
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    nn += 1;
        //}
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (nn > 1)
            {
                nn -= 1;
            }
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    homo_mode = !homo_mode;
        //}
        if (Input.GetKey(KeyCode.R))
        {
            homo_strength *= 1.1f;
        }
        if (Input.GetKey(KeyCode.F))
        {
            homo_strength *= 0.9f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            conti_shoot = !conti_shoot;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            if (angle < 89)
            {
                angle += 1;
            }
        }
        if (Input.GetKey(KeyCode.H))
        {
            if (angle > 15)
            {
                angle -= 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            shooting_mode = !shooting_mode;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            bounce_mode =!bounce_mode;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            stick_mode = !stick_mode;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            turret_mode = !turret_mode;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            damage += 1;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            damage -= 1;
        }
    }

    public float returnCurrDamage()
    {
        return damage;
    }

    public float returnCurrBalls()
    {
        return nn;
    }

    public void increaseNumberofBalls()
    {
        nn += 1;
        Debug.Log("yeet more balls");
    }

    public void increaseBallSize()
    {
        ss *= 1.1f;
        Debug.Log("yeet bigger balls");
    }

    public void toggleStickyMode()
    {
        stick_mode = true;
    }

    public void toggleHomingMode()
    {
        homo_mode = !homo_mode;
    }

public IEnumerator FireCannonAtPoint(Vector3 point,float t)
    {
        yield return new WaitForSeconds(t);

        //Debug.Log(temp_angle);
        //Debug.Log(Vector3.Angle(new Vector3(dir.x, 0, dir.z), dir));

        Rigidbody temp = Instantiate(cannonballInstance, transform.position+new Vector3(0,1.5f,0), transform.rotation);
        temp.transform.localScale *= ss;


        Vector3 velocity;
        if (shooting_mode)
        {
            velocity = (point - transform.position - new Vector3(0, 1.5f, 0)).normalized * 20;
            temp.useGravity = false;
        }
        else if (float_ball)
        {
            velocity = new Vector3(0,0,0);
            temp.useGravity = false;
        }
        else
        {
            velocity = BallisticVelocity(point, angle);
        }
        //Debug.Log("Firing at " + point + " velocity " + velocity);


        //Destroy(temp, 1);      freeze after one second
        Destroy(temp.gameObject, 10);
        temp.velocity = velocity;
        cannonslist.Add(temp);
        //cannonballInstance.velocity = velocity;
    }

    public Vector3 BallisticVelocity(Vector3 destination, float angle)
    {

        Vector3 dir = destination - transform.position - new Vector3(0, 1.5f, 0); // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        //float TotalSpeed = (1 / Mathf.Cos(a)) * Mathf.Sqrt(0.5F * dist * dist * Physics.gravity.magnitude / (height + Mathf.Tan(a) * dist));
       
        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }
}



// https://unity3d.college/2017/06/30/unity3d-cannon-projectile-ballistics/