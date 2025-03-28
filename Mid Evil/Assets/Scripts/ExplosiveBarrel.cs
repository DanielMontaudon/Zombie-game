using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    Rigidbody rb;
    public Spell barrelBlast;
    public Transform lastHitBy;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void LightningHit(Transform player, Vector3 hitPosition, float force)
    {
        lastHitBy = player;
        //transform.position
        rb.AddForce((hitPosition - player.position).normalized * (force * 1000));

        Invoke("Explode", 1f);
    }

    public void FireHit(Transform player)
    {
        lastHitBy = player;
        Explode();
    }

    public void Explode()
    {
        //Sphere Check
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] enemys = Physics.OverlapSphere(transform.position, barrelBlast.range, enemyLayer);

        //Add same sphere check for interactables? barrels persay??

        //If enemys are within range
        if (enemys.Length > 0)
        {
            //for each enemy, see if they are behind structures/walls when in range
            foreach (Collider enemy in enemys)
            {
                RaycastHit spellHit;
                Physics.Linecast(transform.position, enemy.transform.position, out spellHit);

                //Otherwise apply damage and such
                if (spellHit.collider.CompareTag("Enemy"))
                {
                    EnemyMovement em = enemy.gameObject.GetComponent<EnemyMovement>();
                    EnemyAttributes ea = enemy.gameObject.GetComponent<EnemyAttributes>();

                    ea.ApplyDamage(barrelBlast.damage * 2);

                    em.target = lastHitBy;
                    em.Knockback(transform.position, barrelBlast.knockback);

                }

            }
        }
        Destroy(gameObject, 0.1f);
    }
}
