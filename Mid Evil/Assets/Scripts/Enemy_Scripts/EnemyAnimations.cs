using UnityEngine;
using Animancer;
public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] AnimationClip enemyRunning;
    [SerializeField] AnimancerComponent animancer;
    private EnemyMovement em;
    private EnemyMovement.EnemyState enemyState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        em = gameObject.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyState = em.state;
        if(em.state == EnemyMovement.EnemyState.chasing)
        {
            animancer.Play(enemyRunning);
        }
    }
}
