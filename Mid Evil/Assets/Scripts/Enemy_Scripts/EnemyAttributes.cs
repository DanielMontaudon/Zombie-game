using UnityEngine;

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
            Destroy(this.gameObject);
        }
    }
}
