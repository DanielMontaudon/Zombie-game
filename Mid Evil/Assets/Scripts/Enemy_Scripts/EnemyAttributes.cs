using UnityEngine;
using UnityEngine.AI;

using System.Collections;
using System.Collections.Generic;
using INab.Common;

public class EnemyAttributes : MonoBehaviour
{
    public float enemyHealth = 100f;
    public float enemyArmor = 100f;
    public float enemyRange = 1f;
    public float enemyDamage = 2f;
    public float enemyAttackSpeed = 1f;

    [SerializeField] private EnemyType enemyType = EnemyType.regular;
    [SerializeField] private GameObject enemyTypeVFX;
    /*
     * Fire - 0
     * Earth - 1
     * Wind - 2
     * Water - 3
     * Lightning - 4
     * Arcana - 5
     */
    public List<GameObject> enemyAuras = new List<GameObject>();
    //Different types resemble special infected
    public enum EnemyType
    {
        regular,
        fire,
        earth,
        wind,
        water,
        lightning,
        arcane
    }

    [SerializeField] private Transform maskDissolve;

    private bool isDead = false;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        float enemyTypeGenerator = Random.Range(1, 100);
        if (enemyTypeGenerator <= 85)
            enemyType = EnemyType.regular;
        else if(enemyTypeGenerator <= 88)
            enemyType = EnemyType.fire;
        else if (enemyTypeGenerator <= 91)
            enemyType = EnemyType.earth;
        else if (enemyTypeGenerator <= 94)
            enemyType = EnemyType.wind;
        else if (enemyTypeGenerator <= 97)
            enemyType = EnemyType.water;
        else if (enemyTypeGenerator <= 100)
            enemyType = EnemyType.lightning;

        switch (enemyType)
        {
            case EnemyType.fire:
                Instantiate(enemyAuras[0], enemyTypeVFX.transform);
                break;
            case EnemyType.earth:
                //Instantiate(enemyAuras[1], enemyTypeVFX.transform);
                break;
            case EnemyType.wind:
                //Instantiate(enemyAuras[2], enemyTypeVFX.transform);
                break;
            case EnemyType.water:
                Instantiate(enemyAuras[3], enemyTypeVFX.transform);
                break;
            case EnemyType.lightning:
                Instantiate(enemyAuras[4], enemyTypeVFX.transform);
                break;
        }
        enemyMovement = gameObject.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.fire:
                EmitFire();
                break;
            case EnemyType.earth:
                break;
            case EnemyType.wind:
                break;
            case EnemyType.water:
                break;
            case EnemyType.regular:
                break;
        }

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
        
        yield return new WaitForSeconds(2.2f);
        Destroy(this.gameObject);
    }


    private void EmitFire()
    {

    }
}
