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


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(float damage)
    {
        enemyHealth -= damage;

        if(enemyHealth <= 0)
        {
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

        //NEEDS BIG WORK, CHANGE ORDER THINGS HAPPEN MAYBE
        //MAKE SURE WHEN YOU GET A REAL ENEMY MODEL TO MAKE SURE THE MESH IS STANDING UPRIGHT

        InteractiveEffect ie = gameObject.GetComponentInChildren<InteractiveEffect>();
        ie.PlayEffect();
        //Turn everything off and ragdoll
        
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
