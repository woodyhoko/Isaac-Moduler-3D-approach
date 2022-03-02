using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannon_ball : MonoBehaviour
{
    Collider m_ObjectCollider;
    Cannon get_mode;
    bool start_coli = false;
    bool stucked = false;
    int cool_down = 50;
    bool mother_layer = true;
    private GameObject enemy;
    private System.Collections.Generic.List<Rigidbody> cannonslist = new System.Collections.Generic.List<Rigidbody>();
    private Rigidbody cannonballInstance;
    //int start_safe_time=10;
    // Start is called before the first frame update
    void Start()
    {
        cannonballInstance = this.GetComponent<Rigidbody>();
        this.tag = "ball";
        get_mode = GameObject.Find("Player_Model").GetComponent<Cannon>();
        m_ObjectCollider = GetComponent<Collider>();
        m_ObjectCollider.isTrigger = true;
        start_coli = false;
        cool_down = 50;
        stucked = false;
        enemy = GameObject.Find("Cube");
    }
    //void Awake()
    //{
    //    start_safe_time = ;
    //}
    // Update is called once per frame
    void Update()
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
        if (get_mode.homo_mode)
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
                cannonslist[n].velocity += (enemy.transform.position - cannonslist[n].transform.position).normalized * get_mode.homo_strength;
            }
        }

        if (mother_layer && get_mode.turret_mode && cool_down--<0)
        {
            float delay_time = 0.1f;
            if (get_mode.shooting_mode)
            {
                delay_time = 0.1f;
            }
            for (int n = 0; n < get_mode.nn; n++)
            {
                StartCoroutine(FireCannonAtPoint(enemy.transform.position, n * delay_time));
            }
            cool_down = 50;
        }
        //if (start_coli)
        //{
        //    start_safe_time--;
        //    if (start_safe_time < 0)
        //    {
        //        m_ObjectCollider.isTrigger = false;
        //    }
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("hihi");
        if (!start_coli)
        {
            m_ObjectCollider.isTrigger = false;
            start_coli = true;
        }
        //start_safe_time = 10;
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (start_coli && !get_mode.bounce_mode)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    void OnCollisionEnter (Collision other)
    {
        //Debug.LogError("%s",other);
        if (start_coli && !get_mode.bounce_mode && other.gameObject.tag!="ball" && other.gameObject.tag != "player")
        {
            Destroy(gameObject);
        }
        if (start_coli && get_mode.stick_mode && other.gameObject.tag != "ball" && stucked==false && other.gameObject.tag != "player")
        {
            stucked = true;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            this.GetComponent<Rigidbody>().useGravity=false;
            Destroy(this.GetComponent<Rigidbody>(), 0);
            var emptyTemp = new GameObject();
            emptyTemp.transform.parent = other.transform;
            //emptyTemp.transform.localScale = new Vector3(1 / other.transform.localScale.x, 1 / other.transform.localScale.y, 1 / other.transform.localScale.z);
            this.transform.parent = emptyTemp.transform;
            //this.transform.localScale = new Vector3(1/other.transform.localScale.x, 1 / other.transform.localScale.y, 1 / other.transform.localScale.z);
        }
    }


    public IEnumerator FireCannonAtPoint(Vector3 point, float t)
    {
        yield return new WaitForSeconds(t);

        //Debug.Log(temp_angle);
        //Debug.Log(Vector3.Angle(new Vector3(dir.x, 0, dir.z), dir));

        Rigidbody temp = Instantiate(cannonballInstance, transform.position, transform.rotation);
        temp.transform.localScale *= get_mode.ss;


        Vector3 velocity;
        if (get_mode.shooting_mode)
        {
            velocity = (point - transform.position ).normalized * 20;
            temp.useGravity = false;
        }
        else
        {
            velocity = BallisticVelocity(point, get_mode.angle);
        }
        Debug.Log("Firing at " + point + " velocity " + velocity);


        //Destroy(temp, 1);      freeze after one second
        Destroy(temp.gameObject, 10);
        temp.GetComponentInParent<cannon_ball>().mother_layer = false;
        temp.velocity = velocity;
        temp.GetComponentInParent<Transform>().localScale *= 0.5f;
        cannonslist.Add(temp);
        //cannonballInstance.velocity = velocity;
    }


    public Vector3 BallisticVelocity(Vector3 destination, float angle)
    {

        Vector3 dir = destination - transform.position; // get Target Direction
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
