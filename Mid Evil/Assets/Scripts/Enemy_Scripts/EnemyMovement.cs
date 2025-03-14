using UnityEngine;
using UnityEngine.AI;
using System.Collections;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private Rigidbody rb;

    public Transform target;
    public bool targetFound = false;

    Collider[] players;
    public LayerMask playerMask;
    public float triggerRadius = 10f;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (agent.enabled)
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

    public void Knockback(Vector3 forcePosition, float force)
    {
        rb.isKinematic = false;
        agent.enabled = false;

        rb.AddForce((transform.position - forcePosition).normalized * force);
        StartCoroutine(ResetEnemy());
    }

    private IEnumerator ResetEnemy()
    {
        yield return new WaitForSeconds(0.25f);

        rb.isKinematic = true;
        agent.enabled = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);

        Gizmos.DrawSphere(gameObject.transform.position, triggerRadius);
    }

}
