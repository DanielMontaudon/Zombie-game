using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class PlayerCombat : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform playerCam;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Loadout")]
    public Spell leftSpell;
    public Spell rightSpell;
    public Spell specialSpell;
    public Spell defensiveSpell;


    [Header("Keybinds")]
    public KeyCode leftKeybind = KeyCode.Mouse0;
    public KeyCode rightKeybind = KeyCode.Mouse1;
    public KeyCode specialKeybind = KeyCode.Q;
    public KeyCode defensiveKeybind = KeyCode.E;



    float startY;
    PlayerAttributes playerStats;
    PlayerMovement playerMovement;
    public LayerMask whatIsEnemy;
    bool leftOffCooldown = true;
    bool rightOffCooldown = true;
    bool specialOffCooldown = true;
    bool defensiveOffCooldown = true;


    private void Start()
    {
        startY = leftHand.localPosition.y;
        playerStats = gameObject.GetComponent<PlayerAttributes>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(playerCam.position, playerCam.transform.forward * leftSpell.range);
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
        if (Input.GetKey(leftKeybind) && leftOffCooldown && playerStats.mana > leftSpell.manaCost)
        {
            leftOffCooldown = false;
            CastSpell(leftSpell);
            Invoke(nameof(LeftCooldown), leftSpell.cooldown);
        }

        if (Input.GetKey(rightKeybind) && rightOffCooldown && playerStats.mana > rightSpell.manaCost)
        {
            rightOffCooldown = false;
            CastSpell(rightSpell);

            Invoke(nameof(RightCooldown), rightSpell.cooldown);
        }

        if (Input.GetKey(specialKeybind) && specialOffCooldown && playerStats.mana > specialSpell.manaCost)
        {

        }

        if (Input.GetKey(defensiveKeybind) && defensiveOffCooldown && playerStats.mana > defensiveSpell.manaCost)
        {

        }
    }

    //Handle Raycasting and such
    void CastSpell(Spell spell)
    {
        //Lightning Spell
        if (spell.spellType == Spell.damageType.Lightning && playerStats.mana >= spell.manaCost)
        {
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
                    ApplyKnockback(raycastHit.collider);
                    CheckIfAttacked(raycastHit.collider);
                }

                //Add some other spicy tags persay? (Explosive barrels, water)
            }
            playerStats.mana -= spell.manaCost;
        }
        //Air Spell
        else if (spell.spellType == Spell.damageType.Air && playerStats.mana >= spell.manaCost)
        {
            //Maybe the grippy stun or maybe just a dash like jett? defensive
            print("Air casted");
            playerStats.mana -= spell.manaCost;
        }
        //Fire Spell
        else if (spell.spellType == Spell.damageType.Fire && playerStats.mana >= spell.manaCost)
        {
            //Cast AOE Fire Spell, Sphere Check
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
                        ApplyKnockback(enemy);
                        CheckIfAttacked(enemy);
                    }
                    
                }
            }          
            playerStats.mana -= spell.manaCost;
        }
        //Earth Spell
        else if (spell.spellType == Spell.damageType.Earth && playerStats.mana >= spell.manaCost)
        {
            //Melee of L4D knocking back while still generating mana?
            print("Earth casted");
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
    void specialCooldown()
    {
        specialOffCooldown = true;
    }
    void defensiveCooldown()
    {
        defensiveOffCooldown = true;
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

    private void ApplyKnockback(Collider enemy)
    {
        EnemyMovement enemyMovement = enemy.gameObject.GetComponent<EnemyMovement>();
        enemyMovement.Knockback(transform.position);
    }
}
