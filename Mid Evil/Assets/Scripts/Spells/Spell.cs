using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public string spellName;
    public float damage;
    public float range;
    public damageType spellType;
    public float manaCost;
    public float cooldown;
    public float knockback;
    public float stunTime;
    public Sprite abilityIcon;

    public enum damageType
    {
        Lightning,
        Air,
        Fire,
        Earth,
        Water,
        Dash,
        Punch
    }

}
