using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform playerCam;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Loadout")]
    public Spell leftSpell;
    public Spell rightSpell;

    [Header("Keybinds")]
    public KeyCode leftKeybind = KeyCode.Mouse0;
    public KeyCode rightKeybind = KeyCode.Mouse1;

    float startY;
    PlayerAttributes playerStats;
    public LayerMask whatIsEnemy;
    bool leftOffCooldown = true;
    bool rightOffCooldown = true;

    private void Start()
    {
        startY = leftHand.localPosition.y;
        playerStats = gameObject.GetComponent<PlayerAttributes>();
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
    }

    //Handle Raycasting and such
    void CastSpell(Spell spell)
    {
        if (spell.spellType == Spell.damageType.Lightning && playerStats.mana >= spell.manaCost)
        {
            //Fix so you dont shoot through walls
            //Debug.DrawRay(playerCam.position, playerCam.transform.forward * leftSpell.range);
            if (Physics.Raycast(playerCam.position, playerCam.transform.forward, spell.range, whatIsEnemy))
            {
                print("Lightning casted");
            }
            playerStats.mana -= spell.manaCost;
        }
        else if (spell.spellType == Spell.damageType.Air && playerStats.mana >= spell.manaCost)
        {
            print("Air casted");
            playerStats.mana -= spell.manaCost;
        }
        else if (spell.spellType == Spell.damageType.Fire && playerStats.mana >= spell.manaCost)
        {
            print("Fire casted");
            playerStats.mana -= spell.manaCost;
        }
        else if (spell.spellType == Spell.damageType.Earth && playerStats.mana >= spell.manaCost)
        {
            print("Earth casted");
            playerStats.mana -= spell.manaCost;
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
}
