using UnityEngine;

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
        enemyLayer = LayerMask.GetMask("Enemy");

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
                    //Maybe change layer and change code to find sole enemy object and then destroy from there
                    Destroy(enemies[i].gameObject);
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
