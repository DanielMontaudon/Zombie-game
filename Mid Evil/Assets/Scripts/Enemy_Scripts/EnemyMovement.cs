using UnityEngine;
using UnityEngine.AI;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;

    public Transform target;
    public bool targetFound = false;

    Collider[] players;
    public LayerMask playerMask;
    public float triggerRadius = 10f;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(targetFound)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);

        Gizmos.DrawSphere(gameObject.transform.position, triggerRadius);
    }

}
