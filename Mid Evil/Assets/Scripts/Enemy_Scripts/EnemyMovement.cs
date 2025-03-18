using UnityEngine;
using UnityEngine.AI;
using System.Collections;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private Rigidbody rb;

    public Transform target;
    public bool targetFound = false;
    public bool stunned = false;
    public bool knocked = false;

    Collider[] players;
    public LayerMask playerMask;
    public float triggerRadius = 10f;

    //public LayerMask whatIsGround;
    //public bool grounded;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //grounded = Physics.CheckSphere(transform.position, 0.3f, whatIsGround);


        if (!knocked && !stunned)
        {
            if (targetFound)
            {
                FollowTarget();
            }
            else
            {
                CheckRadius();

                //Some slow patrol thing 
                //Patrol();
            }
        }

    }
    private void FollowTarget()
    {
        //agent.destination = target.position;
        agent.SetDestination(target.position);

    }

    private void CheckRadius()
    {
        players = Physics.OverlapSphere(gameObject.transform.position, triggerRadius, playerMask);
        if (players.Length > 0)
        {
            RaycastHit playerHit;
            Physics.Linecast(gameObject.transform.position + (Vector3.up * agent.height), players[0].transform.position, out playerHit);
            //print(players[0].transform.position);

            if (playerHit.collider.CompareTag("Player"))
            {
                target = players[0].transform;
                targetFound = true;
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
        knocked = true;
        rb.isKinematic = false;
        agent.enabled = false;

        rb.AddForce((transform.position - forcePosition).normalized * force);
        StartCoroutine(ResetEnemy(0.25f));
    }

    private IEnumerator ResetEnemy(float time)
    {
        yield return new WaitForSeconds(time);

        rb.isKinematic = true;
        agent.enabled = true;
        knocked = false;

    }

    //lifts enemy into air and resumes chase after landing
    public void StopNav()
    {
        stunned = true;
        rb.isKinematic = false;
        rb.useGravity = false;
        agent.enabled = false;

        rb.AddForce(transform.up * 1000f);

        StartCoroutine(ResumeNav());


    }
    private IEnumerator ResumeNav()
    {
        yield return new WaitForSeconds(1f);
        rb.useGravity = true;

        yield return new WaitForSeconds(1.5f);
        rb.isKinematic = true;
        agent.enabled = true;
        stunned = false;
        //print("Enemy has landed");
        //rb.AddForce(transform.up * -1000f);
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
