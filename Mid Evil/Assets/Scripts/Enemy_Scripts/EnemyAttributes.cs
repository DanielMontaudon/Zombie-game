using UnityEngine;
using UnityEngine.AI;

using System.Collections;

public class EnemyAttributes : MonoBehaviour
{
    public float enemyHealth = 100f;
    public float enemyArmor = 100f;


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
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
