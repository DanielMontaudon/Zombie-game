using UnityEngine;
using FIMSpace.FProceduralAnimation;

public class ChandelierLogic : MonoBehaviour
{

    public LayerMask whatIsGround;
    public Transform chandelierModel;

    public bool grounded = false;
    Rigidbody rb;
    Collider[] enemies;
    LayerMask enemyLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        enemyLayer = LayerMask.GetMask("Ragdoll");

    }

    // Update is called once per frame
    void Update()
    {
        if(!rb.isKinematic && !grounded)
        {
            grounded = Physics.CheckBox(transform.position + (Vector3.down * 0.3f), chandelierModel.localScale / 2.1f, transform.rotation, whatIsGround);
            enemies = Physics.OverlapBox(transform.position + (Vector3.down * 0.3f), chandelierModel.localScale / 2.1f, transform.rotation, enemyLayer);
            if(enemies.Length > 0)
            {
                for(int i = 0; i < enemies.Length; i++)
                {
                    GameObject enemy = enemies[i].gameObject;
                    //Ragdoll and Physical body are seperate so you must get a reference to the physical parent to access data
                    RagdollAnimatorDummyReference enemyReference = enemy.GetComponentInParent<RagdollAnimatorDummyReference>();

                    EnemyMovement enemyMovement = enemyReference.ParentComponent.GetComponent<EnemyMovement>();
                    enemyMovement.Knockback(enemyMovement.transform.position, 0, 1);

                    Destroy(enemyMovement.gameObject, 0.6f);
                    //Maybe change layer and change code to find sole enemy object and then destroy from there
                    //Destroy(enemy);
                    //print(enemies[i].name);
                }
            }
        }

        if(grounded)
        {
            Invoke(nameof(DestroyChandelier), 1f);
        }

    }

    public void DropChandelier()
    {
        rb.isKinematic = false;
    }

    private void DestroyChandelier()
    {
        Destroy(this.gameObject);
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + (Vector3.down * 0.3f), chandelierModel.localScale / 1.1f);
    }
}
