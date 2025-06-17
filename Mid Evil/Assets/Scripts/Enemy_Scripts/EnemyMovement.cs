using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using FIMSpace.FProceduralAnimation;



public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent agent;
    private EnemyAnimations eAnim;
    private EnemyAttributes ea;
    private RagdollAnimator2 ra;
    private Animator anim;

    public Transform target;
    public bool stunned = false;
    public bool readyToAttack = true;
    /*
    public bool knocked = false;
    */
    public float ragdollBlendValue;

    Collider[] players;
    public LayerMask playerMask;
    public LayerMask groundMask;
    public float triggerRadius = 10f;

    public EnemyState state;
    public enum EnemyState
    {
        idle,
        chasing,
        stunned,
        knocked,
        attacking,
        gettingUpKnock,
        gettingUpStun,
        rotating
    }

    private Coroutine attackingCoroutine;

    void Start()
    {
        state = EnemyState.idle;
        agent = gameObject.GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        ea = gameObject.GetComponent<EnemyAttributes>();
        eAnim = gameObject.GetComponent<EnemyAnimations>();
        ra = gameObject.GetComponent<RagdollAnimator2>();
        ragdollBlendValue = ra.RagdollBlend;
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

        StartCoroutine(ChaseReset());
        //agent.enabled = true;
        //state = EnemyState.chasing;

    }

    private IEnumerator ChaseReset()
    {
        yield return new WaitForSeconds(ea.enemyAttackSpeed);

        if (state == EnemyState.attacking)
        {
            agent.enabled = true;
            state = EnemyState.chasing;
        }
    }

    private void ResetAttack()
    {
        readyToAttack = true;
    }

    private void LookAtTarget()
    {
        state = EnemyState.rotating;
        agent.enabled = false;

        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        bool isTurningRight = Vector3.Cross(transform.rotation * Vector3.forward, targetRotation * Vector3.forward).y > 0;
        //Negative Y value = Turning LEFT, Positive y value = Turning RIGHT
        print("Right(+)/Left(-) = " + Vector3.Cross(transform.rotation*Vector3.forward, targetRotation*Vector3.forward).y);
        print("Angle = " + Quaternion.Angle(transform.rotation, targetRotation));
        eAnim.RotateAnimation(Quaternion.Angle(transform.rotation, targetRotation), isTurningRight);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 30f);
        //transform.rotation = targetRotation;

        StartCoroutine(LookAtReset());
    }

    private IEnumerator LookAtReset()
    {
        yield return new WaitForSeconds(0.05f);

        if (state == EnemyState.rotating)
        {
            agent.enabled = true;
            state = EnemyState.chasing;
        }
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
            //Physics.Raycast(transform.position + (Vector3.up * 1.5f), transform.forward, ea.enemyRange, playerMask)
            if (Physics.SphereCast(new Ray(transform.position + (Vector3.up * 1.5f), transform.forward), .5f, ea.enemyRange, playerMask))
            {
                readyToAttack = false;
                eAnim.AttackAnimation();
                SwingAttack();
                
                Invoke("ResetAttack", ea.enemyAttackSpeed);
            }
            //else turn towards player
            else
            {
                LookAtTarget();
                //print("Rotating");
            }
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
    public void Knockback(Vector3 forcePosition, float force, float stunTime)
    {
        state = EnemyState.knocked;
 
        //ra.User_SetAllKinematic(false);

        //anim.enabled = false;
        agent.enabled = false;
        ra.User_SwitchFallState(false);
        ra.RagdollBlend = .85f;
        ra.User_AddAllImpact((gameObject.transform.position - forcePosition).normalized * force, 0.01f, ForceMode.Force);
        ra.User_FadeMusclesPower(0.4f, 0.1f);

        StartCoroutine(ResetEnemy(stunTime));
    }

    //Reset physics to knockbacked enemy
    private IEnumerator ResetEnemy(float time)
    {
        //agent.Warp(ra.User_GetPosition_Center());
        yield return new WaitForSeconds(time + Random.Range(0f, 0.5f));
        RaycastHit belowHips = ra.User_ProbeGroundBelowHips(groundMask);
        //print(belowHips.point);
        agent.Warp(belowHips.point);
        ra.User_TransitionToStandingMode();
        ra.User_FadeMusclesPower(1f, 0.02f);
        state = EnemyState.gettingUpKnock;
        //ra.User_TransitionToStandingMode();
        //agent.Warp(ra.User_GetPosition_Center());
        //ra.User_SetAllKinematic(true);

        //anim.enabled = true;
        //ra.User_FadeMusclesPower(1f, 0.02f);
        ra.RagdollBlend = ragdollBlendValue;
        eAnim.GetUpAnimation();
        yield return new WaitForSeconds(2f);
        //ra.User_FadeMusclesPower(1f);
        //ra.User_TransitionToStandingMode();

        //anim.enabled = true;
        if (state == EnemyState.gettingUpKnock)
        {
            agent.enabled = true;
            state = EnemyState.chasing;
        }

    }

    //lifts enemy into air and resumes chase after landing
    public void StopNav(Transform newTarget)
    {
        state = EnemyState.stunned;

        //ra.User_SetAllKinematic(false);
        ra.User_SwitchAllBonesUseGravity(false);

        //anim.enabled = false;
        agent.enabled = false;

        //Stop Enemy
        ra.User_SetAllVelocity(Vector3.zero);
        //Unlock Anchor Bone
        ra.User_SwitchFallState(false);
        //Make Ragdoll Blend with Physical animated body 
        ra.RagdollBlend = .85f;
        //Add Force Upward
        ra.User_AddAllImpact(Vector3.up * 0.7f, 0.1f, ForceMode.Impulse);
        ra.User_FadeMusclesPower(0.4f, 0.2f);

        StartCoroutine(ResumeNav(newTarget));


    }

    //Resume Agent after enemy has landed
    private IEnumerator ResumeNav(Transform newTarget)
    {
        yield return new WaitForSeconds(1f);
        ra.User_SwitchAllBonesUseGravity(true);
        ra.User_AddAllImpact(Vector3.up * -0.7f,0.1f,ForceMode.Impulse);
        yield return new WaitForSeconds(1.75f + Random.Range(0f,0.5f));
        ra.User_TransitionToStandingMode();
        state = EnemyState.gettingUpStun;
        ra.User_FadeMusclesPower(1f,0.2f);
        ra.RagdollBlend = ragdollBlendValue;
        eAnim.GetUpAnimation();
        //ra.User_TransitionToStandingMode();
        //ra.User_SwitchFallState(true);
        //agent.Warp(ra.User_GetPosition_Center());
        //ra.User_SetAllKinematic(true);

        yield return new WaitForSeconds(2f);
        //ra.User_FadeMusclesPower(1f);
        target = newTarget;

        //anim.enabled = true;
        if (state == EnemyState.gettingUpStun)
        {
            agent.enabled = true;
            state = EnemyState.chasing;
        }
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
