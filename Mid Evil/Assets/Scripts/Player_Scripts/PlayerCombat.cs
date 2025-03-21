using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class PlayerCombat : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform playerCam;
    public Transform orientation;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Loadout")]
    public Spell leftSpell;
    public Spell rightSpell;
    public Spell specialSpell;
    public Spell defensiveSpell;
    public Spell dashSpell;



    [Header("Keybinds")]
    public KeyCode leftKeybind = KeyCode.Mouse0;
    public KeyCode rightKeybind = KeyCode.Mouse1;
    public KeyCode specialKeybind = KeyCode.Q;
    public KeyCode defensiveKeybind = KeyCode.E;
    public KeyCode dashKeybind = KeyCode.LeftAlt;



    float startY;
    PlayerAttributes playerStats;
    PlayerMovement playerMovement;
    //public LayerMask whatIsEnemy;
    bool leftOffCooldown = true;
    bool rightOffCooldown = true;
    bool specialOffCooldown = true;
    bool defensiveOffCooldown = true;
    bool dashOffCooldown = true;


    private void Start()
    {
        startY = leftHand.localPosition.y;
        playerStats = gameObject.GetComponent<PlayerAttributes>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(playerCam.position, playerCam.transform.forward * leftSpell.range);
        MovementHandle();
        SpellLogic();
    }
    //Raise hand/animation
    void MovementHandle()
    {
        if (Input.GetKey(leftKeybind))
        {
            Vector3 raisedHeight = new Vector3(leftHand.localPosition.x, startY + 0.2f, leftHand.localPosition.z);
            leftHand.localPosition = Vector3.Lerp(leftHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
        else
        {
            Vector3 raisedHeight = new Vector3(leftHand.localPosition.x, startY, leftHand.localPosition.z);
            leftHand.localPosition = Vector3.Lerp(leftHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }

        if (Input.GetKey(rightKeybind))
        {
            Vector3 raisedHeight = new Vector3(rightHand.localPosition.x, startY + 0.2f, rightHand.localPosition.z);
            rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
        else
        {
            Vector3 raisedHeight = new Vector3(rightHand.localPosition.x, startY, rightHand.localPosition.z);
            rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, raisedHeight, Time.deltaTime * 10f);
        }
    }

    //if spell can be casted, spell cooldown if held down
    void SpellLogic()
    {
        //Primary Fire
        if (Input.GetKey(leftKeybind) && leftOffCooldown && playerStats.mana > leftSpell.manaCost)
        {
            leftOffCooldown = false;
            CastSpell(leftSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(LeftCooldown), leftSpell.cooldown);
        }
        
        //Secondary Fire
        if (Input.GetKey(rightKeybind) && rightOffCooldown && playerStats.mana > rightSpell.manaCost)
        {
            rightOffCooldown = false;
            CastSpell(rightSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(RightCooldown), rightSpell.cooldown);
        }

        //Special Ability
        if (Input.GetKey(specialKeybind) && specialOffCooldown && playerStats.mana > specialSpell.manaCost)
        {
            specialOffCooldown = false;
            CastSpell(specialSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(SpecialCooldown), specialSpell.cooldown);

        }

        //Defensive Ability
        if (Input.GetKey(defensiveKeybind) && defensiveOffCooldown && playerStats.mana > defensiveSpell.manaCost)
        {
            defensiveOffCooldown = false;
            CastSpell(defensiveSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(DefensiveCooldown), defensiveSpell.cooldown);
        }

        //Dash Ability
        if (Input.GetKey(dashKeybind) && dashOffCooldown && playerStats.mana > dashSpell.manaCost)
        {
            dashOffCooldown = false;
            CastSpell(dashSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(DashCooldown), dashSpell.cooldown);
        }
    }

    //Handle Raycasting and such
    void CastSpell(Spell spell)
    {
        //Lightning Spell
        if (spell.spellType == Spell.damageType.Lightning && playerStats.mana >= spell.manaCost)
        {
            print("Lightning casted");
            RaycastHit raycastHit;
            //Cast Ray straight in front of player 
            bool rayHit = Physics.Raycast(playerCam.position, playerCam.transform.forward, out raycastHit, spell.range);
            
            //If something was hit
            if (rayHit)
            {
                //If that something was a Enemy
                if(raycastHit.collider.CompareTag("Enemy"))
                {
                    //do something with enemy hit (take damage, shock, apply force)
                    print("Lightning casted on: " + raycastHit.collider.tag);
                    DealDamage(raycastHit.collider, spell.damage);
                    ApplyKnockback(raycastHit.collider, spell.knockback);
                    CheckIfAttacked(raycastHit.collider);
                }

                //Add some other spicy tags persay? (Explosive barrels, water)
            }
            playerStats.mana -= spell.manaCost;
        }
        //Air Spell - Tornado that lifts enemies for set time
        else if (spell.spellType == Spell.damageType.Air && playerStats.mana >= spell.manaCost)
        {
            print("Air casted");
            //LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            //RaycastHit[] raycastHits = Physics.RaycastAll(playerCam.position, playerCam.transform.forward, spell.range);
            RaycastHit[] raycastHits = Physics.SphereCastAll(playerCam.position, 1f, orientation.transform.forward, spell.range);

            foreach (RaycastHit hit in raycastHits)
            {
                if(hit.collider.CompareTag("Enemy"))
                {
                    CCEnemy(hit.collider);
                }
                //print(hit.collider.tag);
            }
            playerStats.mana -= spell.manaCost;
        }
        //Fire Spell - AoE Sphere that does massive damage
        else if (spell.spellType == Spell.damageType.Fire && playerStats.mana >= spell.manaCost)
        {
            //Sphere Check
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] enemys = Physics.OverlapSphere(gameObject.transform.position, spell.range, enemyLayer);

            //Add same sphere check for interactables? barrels persay??

            //If enemys are within range
            if(enemys.Length > 0)
            {
                //for each enemy, see if they are behind structures/walls when in range
                foreach(Collider enemy in enemys)
                {
                    RaycastHit spellHit;
                    Physics.Linecast(gameObject.transform.position + (Vector3.up * playerMovement.playerHeight / 2f), enemy.transform.position + (Vector3.up * playerMovement.playerHeight / 2f), out spellHit);

                    //Otherwise apply damage and such
                    if(spellHit.collider.CompareTag("Enemy"))
                    {
                        DealDamage(enemy, spell.damage);
                        ApplyKnockback(enemy, spell.knockback);
                        CheckIfAttacked(enemy);
                    }
                    
                }
            }          
            playerStats.mana -= spell.manaCost;
        }
        //Earth Spell - Defensive stance that roots player and makes player invulnrable, healing and regen in the process
        else if (spell.spellType == Spell.damageType.Earth && playerStats.mana >= spell.manaCost)
        {
            print("Earth casted");
            playerStats.mana -= spell.manaCost;
        }
        //Dash Spell - Dash in direction player is facing pushing enemies aside
        else if (spell.spellType == Spell.damageType.Dash && playerStats.mana >= spell.manaCost)
        {
            StartCoroutine(Dash());
            
            print("Dash casted");
            playerStats.mana -= spell.manaCost;
        }
        //Water Spell ??

    }

    void LeftCooldown()
    {
        leftOffCooldown = true;
    }

    void RightCooldown()
    {
        rightOffCooldown = true;
    }
    void SpecialCooldown()
    {
        specialOffCooldown = true;
    }
    void DefensiveCooldown()
    {
        defensiveOffCooldown = true;
    }

    void DashCooldown()
    {
        dashOffCooldown = true;
    }

    private void CheckIfAttacked(Collider collider)
    {
        GameObject enemy = collider.gameObject;
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if(!enemyMovement.targetFound)
        {
            enemyMovement.target = this.transform;
            enemyMovement.targetFound = true;
        }

    }

    private void DealDamage(Collider collider, float damage)
    {
        GameObject enemy = collider.gameObject;
        EnemyAttributes enemyAttributes = enemy.GetComponent<EnemyAttributes>();
        enemyAttributes.ApplyDamage(damage);
    }

    private void ApplyKnockback(Collider enemy, float force)
    {
        EnemyMovement enemyMovement = enemy.gameObject.GetComponent<EnemyMovement>();
        if(enemyMovement.state != EnemyMovement.EnemyState.stunned)
            enemyMovement.Knockback(transform.position, force);
    }

    private IEnumerator Dash()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 1;
        rb.AddForce(playerMovement.orientation.forward * 50f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.15f);

        rb.mass = 5;
        rb.useGravity = true;
    }

    private void CCEnemy(Collider enemy)
    {
        EnemyMovement enemyMovement = enemy.gameObject.GetComponent<EnemyMovement>();
        enemyMovement.StopNav();
    }



    private void OnDrawGizmos()
    {
        //Lightning Debug
        Debug.DrawRay(playerCam.position, playerCam.transform.forward * leftSpell.range);

        //Air Debug
        Debug.DrawRay(orientation.position, orientation.transform.forward * rightSpell.range);

        //Fire Debug
        Gizmos.color = new Color(1f, 1f, 0.5f, 0.25f);
        Gizmos.DrawSphere(gameObject.transform.position, specialSpell.range);

    }
}
