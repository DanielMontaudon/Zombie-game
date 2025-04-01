using UnityEngine;

public class HazardAreaLogic : MonoBehaviour
{
    
    [SerializeField] private Vector3 puddleSize;
    public float damageTicTimer = 1f;
    float timer = 0;
    public float damageFromSpell;
    public bool activated = false;
    public GameObject physicalEffect;
    public Material lightningEffect;
    public Material fireEffect;



    private void Update()
    {
        if(activated)
        {
            HazardDamage();
        }
    }
    private void HazardDamage()
    {
        timer += Time.deltaTime;
        if(timer > damageTicTimer)
        {
            Collider[] hazardCol = Physics.OverlapBox(transform.position + Vector3.up * 0.5f, puddleSize / 2f);
            if (hazardCol.Length > 0)
            {
                foreach (Collider collider in hazardCol)
                {
                    if (collider.CompareTag("Player"))
                    {
                        collider.GetComponent<PlayerAttributes>().ApplyDamage(damageFromSpell);
                    }
                    else if (collider.CompareTag("Enemy"))
                    {
                        collider.GetComponent<EnemyAttributes>().ApplyDamage(damageFromSpell);
                    }
                }
            }
            timer = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Vector3.up * 0.5f, puddleSize);
    }


}
