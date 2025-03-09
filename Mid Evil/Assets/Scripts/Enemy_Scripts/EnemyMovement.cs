using UnityEngine;
using UnityEngine.AI;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private SphereCollider triggerRadius;

    public Transform target;
    public bool targetFound = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        triggerRadius = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetFound)
        {
            FollowTarget();
        }
        else
        {

        }
    }
    private void FollowTarget()
    {
        agent.destination = target.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            target = other.transform;
            targetFound = true;
        }
    }

}
