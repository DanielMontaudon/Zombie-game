using UnityEngine;
using UnityEngine.AI;

using System.Collections;
using INab.Common;

public class EnemyAttributes : MonoBehaviour
{
    public float enemyHealth = 100f;
    public float enemyArmor = 100f;
    public float enemyRange = 1f;
    public float enemyDamage = 2f;
    public float enemyAttackSpeed = 1f;

    [SerializeField] private Transform maskDissolve;

    private bool isDead = false;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            maskDissolve.position = enemyMovement.PointBelowEnemyHips();
        }
    }

    public void ApplyDamage(float damage)
    {
        enemyHealth -= damage;

        if(enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(KillEnemy());
            //Add some obvious fluidity
            //coroutines help
            //Destroy(this.gameObject);
        }
    }

    private IEnumerator KillEnemy()
    {
        EnemyMovement em = gameObject.GetComponent<EnemyMovement>();
        em.isDead = true;
        em.state = EnemyMovement.EnemyState.dead;
        yield return new WaitForSeconds(2f);

        //NEEDS BIG WORK, CHANGE ORDER THINGS HAPPEN MAYBE
        //MAKE SURE WHEN YOU GET A REAL ENEMY MODEL TO MAKE SURE THE MESH IS STANDING UPRIGHT
        //em.WarpAgent();
        InteractiveEffect ie = gameObject.GetComponentInChildren<InteractiveEffect>();
        ie.PlayEffect();
        //Turn everything off and ragdoll
        
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
