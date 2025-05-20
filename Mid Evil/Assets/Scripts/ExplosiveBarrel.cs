using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    /*
     * Maybe create a way for each of the elements to interact with the barrel for later roguelike aspects
    */
    Rigidbody rb;
    public Spell barrelBlast;
    public Transform lastHitBy;
    public bool primed = false;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void LightningHit(Transform player, Vector3 hitPosition, float force)
    {
        if(!primed)
        {
            primed = true;

            lastHitBy = player;
            //transform.position
            rb.useGravity = false;
            rb.AddTorque((hitPosition - player.position).normalized * force, ForceMode.Impulse);
            //rb.AddForce(hitPosition.normalized * (force * 1000));
            rb.AddForce((hitPosition - player.position).normalized * (force * 1000));

            Invoke("Explode", 1f);
        }
    }

    public void FireHit(Transform player)
    {
        if (!primed)
        {
            primed = true;

            lastHitBy = player;
            Invoke("Explode", 0.1f);
        }
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
            print(enemys.Length);
            //for each enemy, see if they are behind structures/walls when in range
            foreach (Collider enemy in enemys)
            {
                //print(enemy.name);
                RaycastHit spellHit;
                bool lineHit = Physics.Linecast(transform.position, enemy.transform.position, out spellHit);

                if(lineHit)
                {
                    //Otherwise apply damage and such
                    if (spellHit.collider.CompareTag("Enemy"))
                    {
                        EnemyMovement em = enemy.gameObject.GetComponentInParent<EnemyMovement>();
                        EnemyAttributes ea = enemy.gameObject.GetComponentInParent<EnemyAttributes>();
                        //print(barrelBlast.damage);
                        ea.ApplyDamage(barrelBlast.damage);

                        em.target = lastHitBy;
                        em.Knockback(transform.position, barrelBlast.knockback, barrelBlast.stunTime);

                    }
                }

            }
        }
        Destroy(gameObject, 0.1f);
    }
}
