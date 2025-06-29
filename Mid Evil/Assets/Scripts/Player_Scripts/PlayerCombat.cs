using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using FIMSpace.FProceduralAnimation;



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
    public Spell tornadoSpell;
    public Spell dashSpell;



    [Header("Keybinds")]
    public KeyCode leftKeybind = KeyCode.Mouse0;
    public KeyCode rightKeybind = KeyCode.Mouse1;
    public KeyCode specialKeybind = KeyCode.Q;
    public KeyCode defensiveKeybind = KeyCode.E;
    public KeyCode tornadoKeybind = KeyCode.F;
    public KeyCode dashKeybind = KeyCode.LeftAlt;

    [Header("VFX")]
    public GameObject leftFirePoint;
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject lightningEffectToSpawn;

    float startY;
    PlayerAttributes playerStats;
    PlayerMovement playerMovement;
    ResourceBar resourceBar;
    //public LayerMask whatIsEnemy;
    public bool leftOffCooldown = true;
    public bool rightOffCooldown = true;
    public bool specialOffCooldown = true;
    public bool defensiveOffCooldown = true;
    public bool tornadoOffCooldown = true;
    public bool dashOffCooldown = true;


    private void Start()
    {
        startY = leftHand.localPosition.y;
        playerStats = gameObject.GetComponent<PlayerAttributes>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        resourceBar = gameObject.GetComponentInChildren<ResourceBar>();
        lightningEffectToSpawn = vfx[0];

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
        //Primary Ability (Lightning)
        if (Input.GetKey(leftKeybind) && leftOffCooldown && playerStats.mana > leftSpell.manaCost)
        {
            leftOffCooldown = false;
            CastSpell(leftSpell);
            SpawnVFX();
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(LeftCooldown), leftSpell.cooldown);
        }
        
        //Secondary Ability (Tornado) Punch
        if (Input.GetKey(rightKeybind) && rightOffCooldown && playerStats.mana > rightSpell.manaCost)
        {
            rightOffCooldown = false;
            CastSpell(rightSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(RightCooldown), rightSpell.cooldown);
        }

        //Special Ability (Fire)
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

        //Tornado Ability
        if (Input.GetKey(tornadoKeybind) && tornadoOffCooldown && playerStats.mana > tornadoSpell.manaCost)
        {
            tornadoOffCooldown = false;
            CastSpell(tornadoSpell);
            //Add some UI elements for CD (function to create a timer)
            Invoke(nameof(TornadoCooldown), tornadoSpell.cooldown);
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
        if (playerStats.stasis != true)
        {
            //Bolt Spell
            //spell.spellType == Spell.damageType.Lightning
            if (spell.spellName.Equals("Bolt") && playerStats.mana >= spell.manaCost)
            {
                //ADD A DEBUFF COUNTER INSTEAD OF CONTINUOS KNOCKBACK
                //print("Bolt casted");
                RaycastHit raycastHit;
                //Cast Ray straight in front of player 
                bool rayHit = Physics.Raycast(playerCam.position, playerCam.transform.forward, out raycastHit, spell.range);

                //If something was hit
                if (rayHit)
                {
                    print(raycastHit.collider.tag);
                    //If Enemy was hit
                    if (raycastHit.collider.CompareTag("Enemy"))
                    {
                        //do something with enemy hit (take damage, shock, apply force)
                        //print("Bolt casted on: " + raycastHit.collider.tag);
                        //Headshot Hitbox
                        if(raycastHit.collider.GetType() == typeof(SphereCollider))
                        {
                            DealDamage(raycastHit.collider, spell.damage * playerStats.headshotMulti);
                        }
                        //Bodyshot Hitbox
                        else if(raycastHit.collider.GetType() == typeof(CapsuleCollider) || raycastHit.collider.GetType() == typeof(BoxCollider))
                        {
                            DealDamage(raycastHit.collider, spell.damage);
                        }
                            //Maybe make knockback into Card
                            //ApplyKnockback(raycastHit.collider, spell.knockback);
                            CheckIfAttacked(raycastHit.collider);
                    }

                    //If Barrel was hit
                    if (raycastHit.collider.CompareTag("Barrel"))
                    {
                        ExplosiveBarrel eb = raycastHit.collider.gameObject.GetComponent<ExplosiveBarrel>();
                        eb.LightningHit(transform, raycastHit.point, spell.knockback);
                    }

                    //If Puddle was hit
                    if (raycastHit.collider.CompareTag("Puddle"))
                    {
                        //print("Bolt casted on: " + raycastHit.collider.tag);
                        ArcanePuddle ap = raycastHit.collider.gameObject.GetComponent<ArcanePuddle>();
                        ap.LightningArcane(spell.damage);
                        //Do arcane stuff
                    }

                    if(raycastHit.collider.CompareTag("Chandelier"))
                    {
                        //print("Bolt casted on: " + raycastHit.collider.tag);
                        ChandelierLogic cl = raycastHit.collider.gameObject.GetComponent<ChandelierLogic>();
                        cl.DropChandelier();
                    }

                        //Add some other spicy tags persay? (Explosive barrels, water)
                }
                playerStats.mana -= spell.manaCost;
            }
            //Air Spell - Tornado that lifts enemies for set time
            //Maybe Not tornado persay but wall of air, think moses
            else if (spell.spellName.Equals("Tornado") && playerStats.mana >= spell.manaCost)
            {
                //print("Tornado casted");
                //LayerMask interactableLayer = LayerMask.GetMask("Interactable");
                LayerMask enemyLayer = LayerMask.GetMask("Enemy");
                //RaycastHit[] raycastHits = Physics.RaycastAll(playerCam.position, playerCam.transform.forward, spell.range);
                RaycastHit[] raycastHits = Physics.SphereCastAll(playerCam.position, 1f, orientation.transform.forward, spell.range, ~enemyLayer);
                float closestWall = float.MaxValue;
                //foreach (RaycastHit hit in raycastHits)
                for(int i = 0; i < raycastHits.Length; i++)
                {
                    if (!raycastHits[i].collider.CompareTag("Player") && !raycastHits[i].collider.CompareTag("Barrel"))
                    {
                        //print(raycastHits[i].collider.tag + ": " + Vector3.Distance(raycastHits[i].point, transform.position));
                        if (!raycastHits[i].collider.CompareTag("Enemy"))
                        {
                            if(Vector3.Distance(transform.position, raycastHits[i].point) < closestWall)
                            {
                                closestWall = Vector3.Distance(transform.position, raycastHits[i].point);
                            }
                        }
                    }
                }
                //print("Closest Wall Located at: " + closestWall);
                for (int j = 0; j < raycastHits.Length; j++)
                {
                    if (raycastHits[j].collider.CompareTag("Enemy") && Vector3.Distance(transform.position, raycastHits[j].point) < closestWall)
                    {
                        //CheckIfAttacked(raycastHits[j].collider);
                        LiftEnemy(raycastHits[j].collider);
                    }
                }
            
                playerStats.mana -= spell.manaCost;
            }
            //Nova Spell - AoE Sphere that does massive damage
            else if (spell.spellName.Equals("Nova") && playerStats.mana >= spell.manaCost)
            {
                //print("Nova Casted");
                //Sphere Check
                LayerMask enemyLayer = LayerMask.GetMask("Ragdoll");
                Collider[] enemys = Physics.OverlapSphere(transform.position, spell.range, enemyLayer);

                //Add same sphere check for interactables? barrels persay??
                LayerMask interactableLayer = LayerMask.GetMask("Interactable");
                Collider[] interactables = Physics.OverlapSphere(transform.position, spell.range, interactableLayer);

                //If interactables (barrels, oil/arcane slick)
                if(interactables.Length > 0)
                {
                    foreach(Collider interactable in interactables)
                    {
                        RaycastHit spellHit;
                        Physics.Linecast(transform.position + (Vector3.up * playerMovement.playerHeight / 1.5f), interactable.transform.position, out spellHit, ~enemyLayer);

                        if (spellHit.collider.CompareTag("Barrel"))
                        {
                            ExplosiveBarrel eb = spellHit.collider.gameObject.GetComponent<ExplosiveBarrel>();
                            eb.FireHit(transform);
                        }

                        if (spellHit.collider.CompareTag("Puddle"))
                        {
                            ArcanePuddle ap = spellHit.collider.gameObject.GetComponent<ArcanePuddle>();
                            ap.FireArcane(spell.damage);
                        }
                    }
                }

                //If enemys are within range
                if (enemys.Length > 0)
                {
                    //for each enemy, see if they are behind structures/walls when in range
                    foreach (Collider enemy in enemys)
                    {
                        RaycastHit spellHit;
                        //Look for better casting so it doesnt just aim at their feet
                        //+ (Vector3.up * playerMovement.playerHeight / 1.5f)
                        bool lineHit = Physics.Linecast(transform.position + (Vector3.up * playerMovement.playerHeight / 1.5f), enemy.transform.position, out spellHit);

                        if(lineHit)
                        {
                            //print(spellHit.collider.GetType());
                            //Otherwise apply damage and such
                            if (spellHit.collider.CompareTag("Enemy"))
                            {
                                DealDamage(enemy, spell.damage);
                                //ApplyKnockback(enemy, spell.knockback, spell.stunTime);
                                CheckIfAttacked(enemy);
                                ApplyKnockback(enemy, spell.knockback, spell.stunTime);
                            }
                        }

                    }
                }
                playerStats.mana -= spell.manaCost;
            }
            //ArmorInvocate Spell - Defensive stance that roots player and makes player invulnrable, healing and regen in the process
            else if (spell.spellName.Equals("ArmorInvocate") && playerStats.mana >= spell.manaCost)
            {
                //print("ArmorInvocate casted");
                playerMovement.pauseInput = true;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;

                StartCoroutine(Tremor(spell));

                playerStats.mana -= spell.manaCost;
            }
            //Dash Spell - Dash in direction player is facing pushing enemies aside
            else if (spell.spellName.Equals("Dash") && playerStats.mana >= spell.manaCost)
            {
                StartCoroutine(Dash());

                //print("Dash casted");
                playerStats.mana -= spell.manaCost;
            }
            //Punch "Spell" - Uses Stamina to push enemies in vision (cone cast) back a bit
            //zombie_hit_react_F_01 animation clip in mind
            else if (spell.spellName.Equals("Punch") && playerStats.stamina >= spell.manaCost)
            {

            }
            //Water Spell ??
        }

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
    void TornadoCooldown()
    {
        tornadoOffCooldown = true;
    }
    void DashCooldown()
    {
        dashOffCooldown = true;
    }

    private void CheckIfAttacked(Collider collider)
    {
        GameObject enemy = collider.gameObject;
        //Ragdoll and Physical body are seperate so you must get a reference to the physical parent to access data
        RagdollAnimatorDummyReference enemyReference = enemy.GetComponentInParent<RagdollAnimatorDummyReference>();
        EnemyMovement enemyMovement = enemyReference.ParentComponent.GetComponent<EnemyMovement>();
        if(enemyMovement.state == EnemyMovement.EnemyState.idle)
        {
            enemyMovement.target = this.transform;
            enemyMovement.state = EnemyMovement.EnemyState.chasing;
        }

    }

    private void DealDamage(Collider collider, float damage)
    {
        GameObject enemy = collider.gameObject;
        //Ragdoll and Physical body are seperate so you must get a reference to the physical parent to access data
        RagdollAnimatorDummyReference enemyReference = enemy.GetComponentInParent<RagdollAnimatorDummyReference>();
        EnemyAttributes enemyAttributes = enemyReference.ParentComponent.GetComponent<EnemyAttributes>();
        enemyAttributes.ApplyDamage(damage);
    }

    private void ApplyKnockback(Collider collider, float force, float stunTime)
    {
        GameObject enemy = collider.gameObject;
        //Ragdoll and Physical body are seperate so you must get a reference to the physical parent to access data
        RagdollAnimatorDummyReference enemyReference = enemy.GetComponentInParent<RagdollAnimatorDummyReference>();
        EnemyMovement enemyMovement = enemyReference.ParentComponent.GetComponent<EnemyMovement>();
        if(enemyMovement.state != EnemyMovement.EnemyState.stunned)
            enemyMovement.Knockback(transform.position, force, stunTime);
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

    private void LiftEnemy(Collider collider)
    {
        GameObject enemy = collider.gameObject;
        //print(enemy.name);
        //Ragdoll and Physical body are seperate so you must get a reference to the physical parent to access data
        RagdollAnimatorDummyReference enemyReference = enemy.GetComponentInParent<RagdollAnimatorDummyReference>();
        EnemyMovement enemyMovement = enemyReference.ParentComponent.GetComponent<EnemyMovement>();
        enemyMovement.StopNav(transform);
    }

    private IEnumerator Tremor(Spell spell)
    {
        playerStats.stasis = true;
        print("Cast 1");
        int extraRegen = KnockbackLogic(spell);
        playerStats.health += 10 + extraRegen;
        playerStats.mana += 10 + extraRegen;

        yield return new WaitForSeconds(spell.stunTime + 0.1f);

        print("Cast 2");
        extraRegen = KnockbackLogic(spell);
        playerStats.health += 10 + extraRegen;
        playerStats.mana += 10 + extraRegen;

        yield return new WaitForSeconds(spell.stunTime + 0.1f);

        print("Cast 3");
        extraRegen = KnockbackLogic(spell);
        playerStats.health += 10 + extraRegen;
        playerStats.mana += 10 + extraRegen;

        playerMovement.pauseInput = false;
        playerStats.stasis = false;

    }

    private int KnockbackLogic(Spell spell)
    {
        LayerMask enemyLayer = LayerMask.GetMask("Ragdoll");
        Collider[] enemys = Physics.OverlapSphere(transform.position, spell.range, enemyLayer);
        int hitCount = 0;
        if (enemys.Length > 0)
        {
            //for each enemy, see if they are behind structures/walls when in range
            foreach (Collider enemy in enemys)
            {
                RaycastHit spellHit;
                //+ (Vector3.up * playerMovement.playerHeight / 2f)
                bool lineHit = Physics.Linecast(transform.position + (Vector3.up * playerMovement.playerHeight), enemy.transform.position + (Vector3.up * playerMovement.playerHeight / 2f), out spellHit);

                if (lineHit)
                {
                    //Otherwise apply damage and such
                    if (spellHit.collider.CompareTag("Enemy"))
                    {
                        print("hit enemy");
                        hitCount += 1;
                        ApplyKnockback(enemy, spell.knockback, spell.stunTime);
                        CheckIfAttacked(enemy);

                    }
                }

            }
        }

        return hitCount;
    }

    public void SpawnVFX()
    {
        GameObject vfxToSpawn;

        if(leftFirePoint != null)
        {
            vfxToSpawn = Instantiate(lightningEffectToSpawn, leftFirePoint.transform.position, Quaternion.identity);
            vfxToSpawn.transform.localRotation = playerCam.rotation;
        }
        else
        {
            print("No leftFirePoint");
        }
    }



    private void OnDrawGizmos()
    {
        //Lightning Debug
        Debug.DrawRay(playerCam.position, playerCam.transform.forward * leftSpell.range);

        //Air Debug
        Debug.DrawRay(playerCam.position, orientation.transform.forward * rightSpell.range);

        //Fire Debug
        Gizmos.color = new Color(1f, 1f, 0.5f, 0.25f);
        Gizmos.DrawSphere(gameObject.transform.position, specialSpell.range);
        //Debug.DrawRay(transform.position + (Vector3.up * 3f), orientation.transform.forward * 5f);

    }
}
