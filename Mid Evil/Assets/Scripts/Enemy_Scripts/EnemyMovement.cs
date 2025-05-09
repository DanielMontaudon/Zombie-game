using UnityEngine;
using UnityEngine.AI;
using System.Collections;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    //private Rigidbody rb;
    private EnemyKinematics ek;
    private EnemyAttributes ea;

    public Transform target;
    public bool stunned = false;
    public bool readyToAttack = true;
    /*
    public bool knocked = false;
    */

    Collider[] players;
    public LayerMask playerMask;
    public float triggerRadius = 10f;

    public EnemyState state;
    public enum EnemyState
    {
        idle,
        chasing,
        stunned,
        knocked,
        attacking,
        gettingUp
    }

    private Coroutine attackingCoroutine;

    void Start()
    {
        state = EnemyState.idle;
        agent = gameObject.GetComponent<NavMeshAgent>();
        ea = gameObject.GetComponent<EnemyAttributes>();
        ek = gameObject.GetComponent<EnemyKinematics>();

    }

    void Update()
    {
        //print(Time.deltaTime);
        if(state == EnemyState.idle)
        {
            CheckRadius();
        }
        else if (state == EnemyState.chasing)
        {
            FollowTarget();
        }

    }

    private void SwingAttack()
    {

        state = EnemyState.attacking;
        agent.enabled = false;
        
        //print("Attacking");
        RaycastHit hitInfo;
        bool rayHit = Physics.SphereCast(transform.position + (Vector3.up * 1.5f), 1f, transform.forward, out hitInfo, ea.enemyRange);
        //(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask);
        //bool rayHit = Physics.Raycast(gameObject.transform.position + (Vector3.up * 1.5f), gameObject.transform.forward, out hitInfo, ea.enemyRange);
        //Change Raycast to SphereCast persay? 
        if (rayHit)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                //print("hit player");
                PlayerAttributes pa = hitInfo.collider.GetComponent<PlayerAttributes>();
                if (!pa.stasis)
                {
                    pa.ApplyDamage(ea.enemyDamage);
                }
            }
        }

        agent.enabled = true;
        state = EnemyState.chasing;

    }

    private void ResetAttack()
    {
        readyToAttack = true;
    }
    private void FollowTarget()
    {
        //Make Head of enemy transform.LookAt
        //Make body update using clamp motions

        if(Vector3.Distance(target.position, gameObject.transform.position) < (ea.enemyRange) && readyToAttack)
        {
            //Check if player/target is infront of them
            //Physics.Raycast(gameObject.transform.position + (Vector3.up * 1.5f), gameObject.transform.forward, out hitInfo, ea.enemyRange)
            //Physics.SphereCast(new Ray(transform.position + (Vector3.up * 1.5f), transform.forward), 1f, ea.enemyRange, playerMask)
            if (Physics.Raycast(transform.position + (Vector3.up * 1.5f), transform.forward, ea.enemyRange, playerMask))
            {
                readyToAttack = false;
                SwingAttack();
                
                Invoke("ResetAttack", ea.enemyAttackSpeed);
            }
            //else turn towards player
            else
            {
                //print("Rotating");
                Vector3 direction = target.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        else if(agent.updatePosition)
        {
            agent.SetDestination(target.position);
        }
        //agent.destination = target.position;
        //agent.SetDestination(target.position);

    }

    private void CheckRadius()
    {
        //Are there players within the trigger sphere
        players = Physics.OverlapSphere(gameObject.transform.position, triggerRadius, playerMask);
        if (players.Length > 0)
        {
            //Are the players in sight or are there obstacles in the way
            RaycastHit playerHit;
            Physics.Linecast(gameObject.transform.position + (Vector3.up * agent.height), players[0].transform.position, out playerHit);

            //If they are in sight chase first in array
            if (playerHit.collider.CompareTag("Player"))
            {
                target = players[0].transform;
                state = EnemyState.chasing;
                print("Target has been marked");
            }
        }
    }
    /// <summary>
    /// Functions are called from PlayerCombat.cs/CastSpell function
    /// </summary>
    //Apply spell knockback from player position with specified spell force
    public void Knockback(Vector3 forcePosition, float force, float stunTime)
    {
        state = EnemyState.knocked;
 
        ek.turnOffKinematics();
        //agent.updatePosition = false;
        agent.enabled = false;
        ek.chestKnockback(forcePosition, force);
        StartCoroutine(ResetEnemy(stunTime));
    }

    //Reset physics to knockbacked enemy
    private IEnumerator ResetEnemy(float time)
    {
        yield return new WaitForSeconds(time);

        agent.Warp(ek.hipRB.position);
        ek.turnOnKinematics();
        //TRY USING ANIMATOR TO RESET RAGDOLL
        //SAVE POSITION OF JOINTS AND BONES
        //DEACTIVATE RAGDOLL AND ENABLE ANIMATOR
        //SET ANIMATION FROM SAVED POSITIONS TO STAND STRAIGHT

        //agent.updatePosition = true;
        agent.enabled = true;
        state = EnemyState.chasing;

    }

    //lifts enemy into air and resumes chase after landing
    public void StopNav(Transform newTarget)
    {
        state = EnemyState.stunned;
        ek.turnOffKinematics();
        ek.turnOffGravity();

        //agent.updatePosition = false;
        agent.enabled = false;

        ek.liftKinematics(1000f);

        StartCoroutine(ResumeNav(newTarget));


    }

    //Resume Agent after enemy has landed
    private IEnumerator ResumeNav(Transform newTarget)
    {
        yield return new WaitForSeconds(1f);
        ek.turnOnGravity();
        ek.liftKinematics(-1000f);

        yield return new WaitForSeconds(1.5f);
        ek.turnOnKinematics();

        target = newTarget;

        //agent.updatePosition = true;
        agent.enabled = true;

        state = EnemyState.chasing;
    }



    private void OnDrawGizmos()
    {


        //trigger radius
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
        Gizmos.DrawSphere(gameObject.transform.position, triggerRadius);

        //ground check
        Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.3f);

        Gizmos.DrawRay(transform.position + (Vector3.up * 1.5f), transform.forward * 4f);
    }

}
