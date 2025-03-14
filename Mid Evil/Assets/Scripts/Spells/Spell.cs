using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public float damage;
    public float range;
    public damageType spellType;
    public float manaCost;
    public float cooldown;
    public float knockback;

    //Help Denote for ray type
    //Lightning - Primary
    //Air - Secondary
    //Fire - Special
    //Earth - Defensive
    //Dash 
    public enum damageType
    {
        Lightning,
        Air,
        Fire,
        Earth,
        Water,
        Dash
    }

}
