using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class enemy_interactions : MonoBehaviour {
    public int damagePerTick;
    public float damageTickTime;

    private float damageTickCooldown;
    private float enemySpeed = 2;
    private float attackDistance = 2;
    private float playerDistance;
    private float lookAtDistance = 3;

    player player;
    bool isTouchingPlayer = false;
    bool hurtPlayerInitial = false;
    bool shouldHover = false;
    bool shouldHoverPlayer = false;
    bool shouldOilSplatter = false;

    //[SerializeField]
    //public AudioClip MusicClip;
    [SerializeField]
    public AudioSource MusicSource;

    GameObject[] enemyDrops;
    float[] percentages;
    Dictionary<GameObject, float> spawnPercentages = new Dictionary<GameObject, float>();

    GameObject oilDrop;
    private static int oilAmount = 10;

    private void Start()
    {
        //MusicSource.clip = MusicClip;
        player = GameObject.FindWithTag("player").GetComponent<player>();
        enemyDrops = Resources.LoadAll<GameObject>("Spawnables");
        oilDrop = Resources.Load<GameObject>("oil_droplet");
        foreach(GameObject g in enemyDrops)
        {
            spawnPercentages.Add(g, getSpawnPercentage(g.tag));
        }
        if(name.Contains("panemy"))
        {
            shouldHover = true;
            Debug.Log("There is a hover emnemy");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<player>() != null)
        {
            isTouchingPlayer = true;
        }
        if (other.collider.tag == "ball")
        {

            if (GetComponent<enemy>() != null) {}
                GetComponent<enemy>().AdjustEnemyHealth(-player.bulletdmg);
        }
    }

    void OnCollisionExit(Collision other)
    {
        player player = other.collider.GetComponent<player>();

        if (player != null)
        {
            isTouchingPlayer = false;
        }
        hurtPlayerInitial = false;
    }

    void KillEnemy()
    {
        GenerateRandomItem();
        Destroy(gameObject);
        SendMessageUpwards("AdjustScore", 1);
    }

    public void GenerateRandomItem()
    {
        //Debug.Log("hello i am generating something");
        float spawnPerc = Random.Range(0, 1);

        int spawnItem = Mathf.Clamp(Mathf.RoundToInt(Random.Range(0, enemyDrops.Length)), 0, enemyDrops.Length);
        //Debug.Log("Spawn Perc: " + spawnPercentages.Values.ElementAt(spawnItem) + "Spawn Item: " + spawnPercentages.Keys.ElementAt(spawnItem));

        Debug.Log(spawnItem);
        if (spawnPercentages.Values.ElementAt(spawnItem) >= spawnPerc)   //spawnPerc is fitting enought for spawn
        {
            GameObject spawned = Instantiate(spawnPercentages.Keys.ElementAt(spawnItem)) as GameObject;
            spawned.transform.position = this.transform.position;
            Debug.Log(spawned.transform.position);
            Debug.Log("did spawn");
        }
    }

    private void Update()
    {

        if (isTouchingPlayer)
        {
            if(!hurtPlayerInitial)
            {
                MusicSource.Play();
                player.AlterHealth(-1 * damagePerTick);
                hurtPlayerInitial = true;
            } else
            {
                MusicSource.Play();
                damageTickCooldown += Time.deltaTime;
                if (damageTickCooldown >= damageTickTime)
                {
                    player.AlterHealth(-1 * damagePerTick);
                    damageTickCooldown = 0f;
                }
            }
        }

        if(transform.position.y < -20 && name != "oil_droplet")
        {
            KillEnemy();
        }

        //LookAtPlayer();
        if (checkPlayerProximity(this.transform.position, 5) && this.name == "panemy")
        {
            hover();
            splatterOil();
        }

        float step = enemySpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
    }

    bool checkPlayerProximity(Vector3 pos, int radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
        foreach (Collider c in hitColliders)
        {
            if (c.tag == "player")
            {
                return true;
            }
        }
        return false;
    }

    void hover()
    {
        //Debug.Log("moving panemy");
        transform.position = new Vector3(transform.position.x, 10, transform.position.z);
    }

    void splatterOil()
    {
        //damageTickCooldown += Time.deltaTime;
        int countOil = GameObject.FindGameObjectsWithTag("oil").Length;
        if (oilDrop != null && countOil < oilAmount)
        {
            GameObject oilInstance = Instantiate(oilDrop);
            oilInstance.transform.position = this.transform.position;
            //Debug.Log("player perceived position: " + player.transform.position);
            //Vector3 direction = player.transform.position - oilInstance.transform.position;
            //oilInstance.GetComponent<Rigidbody>().velocity = direction.normalized * 2;
            Destroy(oilInstance, 4);
        }
    }

    private float getSpawnPercentage(string tag)
    {
        float perc = 0f;
        if (tag == "health_apple")
        {
            perc = 0.5f;
        }
        if (tag == "balls_glove")
        {
            perc = 0.1f;
        }
        if (tag == "sticky_glue")
        {
            perc = 0.1f;
        }
        if (tag == "balls_mushroom")
        {
            perc = 0.3f;
        }
        if (tag == "homing_magneato_helmet")
        {
            perc = 0.08f;
        }
        return perc;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(this.transform.position, 5);
    }
}
