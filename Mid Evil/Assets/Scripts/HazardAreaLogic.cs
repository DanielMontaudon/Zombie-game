using UnityEngine;

public class HazardAreaLogic : MonoBehaviour
{
    
    [SerializeField] private Vector3 puddleSize;
    public float damageTicTimer = 1f;
    float timer = 0;
    public float damageFromSpell;
    public bool activated = false;

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
                        print(collider.tag);
                    }
                    else if (collider.CompareTag("Enemy"))
                    {
                        print(collider.tag);
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
