using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_script : MonoBehaviour
{
    Transform target;
    Cannon cannon;
    player player;

    //wandering vars
    float wanderSpeed = 2.0f;
    float wanderRotSpeed = 5.0f;
    float wanderRadius = 10.0f;
    float wanderRayDistance = 5.0f;
    float wanderPauseMin = 2.0f;
    float wanderPauseMax = 6.0f;
    Vector3 basePosition;
    Vector3 currentDestination;

    //chase vars
    float chaseDistance = 10.0f;
    float chaseSpeed = 3.0f;
    float chaseRotSpeed = 5.0f;

    //attack vars
    float attackDistance = 3.0f;
    float attackRate = 0.25f;
    int attackTimes = 5;
    int damagePerTick = 3;
    float damageTickTime = 3;
    private float damageTickCooldown;
    bool isTouchingPlayer = false;
    bool hurtPlayerInitial = false;

    //state setup
    enum aiState { wandering, chasing, attacking }
    aiState state;

    void Start()
    {
        player = GameObject.FindWithTag("player").GetComponent<player>();
        InvokeRepeating("StateLogic", 0.0f, 0.01f);
        if (target == null)
            target = GameObject.FindWithTag("player").transform;
        ChooseNextDestination();
        StartCoroutine(StateMachine());
        cannon = GetComponent<Cannon>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<player>() != null)
        {
            isTouchingPlayer = true;
        }
        if (other.collider.tag == "ball")
        {
            if (GetComponent<enemy>() != null) { }
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

    private void Update()
    {
        if (isTouchingPlayer)
        {
            if (!hurtPlayerInitial)
            {
                player.AlterHealth(-1 * damagePerTick);
                hurtPlayerInitial = true;
            }
            else
            {
                damageTickCooldown += Time.deltaTime;
                if (damageTickCooldown >= damageTickTime)
                {
                    player.AlterHealth(-1 * damagePerTick);
                    damageTickCooldown = 0f;
                }
            }
        }
    }

        void StateLogic()
    {
        var distanceToTarget = (target.position - transform.position).sqrMagnitude;
        if (distanceToTarget <= attackDistance * attackDistance)
            state = aiState.attacking;
        else if (distanceToTarget <= chaseDistance * chaseDistance)
            state = aiState.chasing;
        else
            state = aiState.wandering;
    }

    IEnumerator StateMachine()
    {

        while (true)
        {
            switch (state)
            {
                case aiState.wandering:
                    StartCoroutine(Wander());
                    break;
                case aiState.chasing:
                    Chase();
                    break;
                case aiState.attacking:
                    StartCoroutine(Attack());
                    break;
            }
            yield return null;
        }
    }

    IEnumerator Wander()
    {
        RotateToward(currentDestination, wanderRotSpeed);
        MoveForward(wanderSpeed);
        //BroadcastMessage("PlayAnimation", "walk");
        var destPosZeroY = currentDestination;
        var currentPosZeroY = transform.position;
        destPosZeroY.y = 0;
        currentPosZeroY.y = 0;
        if ((destPosZeroY - currentPosZeroY).magnitude < 1.0)
        {
            yield return new WaitForSeconds(Random.Range(wanderPauseMin, wanderPauseMax));
            ChooseNextDestination();
        }
    }

    void ChooseNextDestination()
    {
        Vector2 randOffset = Random.insideUnitCircle * wanderRadius;
        currentDestination = basePosition + new Vector3(randOffset.x, transform.position.y, randOffset.y);
        Debug.DrawLine(transform.position, currentDestination, Color.white);
    }

    void Chase()
    {
        RotateToward(target.position, chaseRotSpeed);
        MoveForward(chaseSpeed);
    }

    IEnumerator Attack()
    {
        //target.GetComponent(PlayerStatus).TakeDamage(20.0);
        if(Vector3.Distance(transform.position, target.position) <= 8)
        throwLadle(attackTimes);
        yield return new WaitForSeconds(attackRate);
    }

    void throwLadle(int times)
    {
        cannon.FireCannonAtPoint(target.position, attackRate);
    }

    void RotateToward(Vector3 targetPos, float rotSpeed)
    {
        targetPos.y = transform.position.y;
        var rotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }

    void MoveForward(float moveSpeed)
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
}
