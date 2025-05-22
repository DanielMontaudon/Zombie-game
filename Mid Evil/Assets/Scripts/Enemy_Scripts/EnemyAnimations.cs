using UnityEngine;
using Animancer;
using FIMSpace.FProceduralAnimation;
public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] AnimationClip enemyRunning;
    [SerializeField] AnimationClip enemyIdle;
    [SerializeField] AnimationClip enemyGetUpBack;
    [SerializeField] AnimationClip enemyGetUpFront;
    [SerializeField] AnimationClip enemyAttack;


    [SerializeField] AnimancerComponent animancer;
    private EnemyMovement em;
    private EnemyMovement.EnemyState enemyState;
    private RagdollAnimator2 ra;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        em = gameObject.GetComponent<EnemyMovement>();
        ra = gameObject.GetComponent<RagdollAnimator2>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyState = em.state;
        if(em.state == EnemyMovement.EnemyState.chasing)
        {
            animancer.Play(enemyRunning, 0.25f);
        }
        else if(em.state == EnemyMovement.EnemyState.idle)
        {
            animancer.Play(enemyIdle, 0.25f);
        }
        else if(em.state != EnemyMovement.EnemyState.attacking && em.state != EnemyMovement.EnemyState.gettingUp)
        {
            animancer.Stop();
        }


    }

    public void GetUpAnimation()
    {
        //animancer.Play(enemyGetUpFront, 0.2f);
        if (ra.User_IsOnBack())
        {
            print("Playing On Back");
            animancer.Play(enemyGetUpBack, 0.2f);
        }
        else
        {
            print("Playing on Front");
            animancer.Play(enemyGetUpFront, 0.2f);
        }
    }

    public void AttackAnimation()
    {
        AnimancerState animancerState = animancer.Play(enemyAttack, 0.2f);
        animancerState.Speed = 2f;
    }
}
