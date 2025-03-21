using UnityEngine;
using UnityEngine.AI;
using System.Collections;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private Rigidbody rb;
    private EnemyAttributes ea;

    public Transform target;
    public bool targetFound = false;
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
        attacking
    }

    private Coroutine attackingCoroutine;

    void Start()
    {
        state = EnemyState.idle;
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        ea = gameObject.GetComponent<EnemyAttributes>();

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

    /*
            RaycastHit hitInfo;
            bool rayHit = Physics.Raycast(gameObject.transform.position + (Vector3.up * 1.5f), gameObject.transform.forward, out hitInfo, ea.enemyRange);
            //Maybe add a WaitForSeconds to give player a small window to dodge the attack instead of guarentee play is hit when swung
            if (rayHit)
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    //print("hit player");
                    PlayerAttributes pa = hitInfo.collider.GetComponent<PlayerAttributes>();
                    pa.ApplyDamage(ea.enemyDamage);
                }
            }
    */
    private void SwingAttack()
    {

        state = EnemyState.attacking;
        agent.enabled = false;
        //add while loop logic
        print("Attacking");
        RaycastHit hitInfo;
        bool rayHit = Physics.Raycast(gameObject.transform.position + (Vector3.up * 1.5f), gameObject.transform.forward, out hitInfo, ea.enemyRange);
        //Maybe add a WaitForSeconds to give player a small window to dodge the attack instead of guarentee play is hit when swung
        if (rayHit)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                //print("hit player");
                PlayerAttributes pa = hitInfo.collider.GetComponent<PlayerAttributes>();
                pa.ApplyDamage(ea.enemyDamage);
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

        if(Vector3.Distance(target.position, gameObject.transform.position) < (ea.enemyRange))
        {
            readyToAttack = false;
            SwingAttack();
            //Look at invoke stuff idk im struggling
            Invoke("ResetAttack", ea.enemyAttackSpeed);
        }
        else if(agent.enabled)
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
    public void Knockback(Vector3 forcePosition, float force)
    {
        //StopCoroutine(SwingAttack());
        state = EnemyState.knocked;
        rb.isKinematic = false;
        agent.enabled = false;

        rb.AddForce((transform.position - forcePosition).normalized * force);
        StartCoroutine(ResetEnemy(0.25f));
    }

    //Reset physics to knockbacked enemy
    private IEnumerator ResetEnemy(float time)
    {
        yield return new WaitForSeconds(time);

        rb.isKinematic = true;
        agent.enabled = true;
        state = EnemyState.chasing;

    }

    //lifts enemy into air and resumes chase after landing
    public void StopNav()
    {
        if(attackingCoroutine != null)
        {
            StopCoroutine(attackingCoroutine);
            attackingCoroutine = null;
        }
        //StopCoroutine(SwingAttack());
        stunned = true;
        state = EnemyState.stunned;
        rb.isKinematic = false;
        rb.useGravity = false;
        agent.enabled = false;

        rb.AddForce(transform.up * 1500f);

        StartCoroutine(ResumeNav());


    }

    //Resume Agent after enemy has landed
    private IEnumerator ResumeNav()
    {
        yield return new WaitForSeconds(1f);
        rb.useGravity = true;
        rb.AddForce(transform.up * -1000f);

        yield return new WaitForSeconds(1.5f);
        rb.isKinematic = true;
        agent.enabled = true;
        stunned = false;
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
    }

}
