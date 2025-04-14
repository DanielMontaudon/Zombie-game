using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public float damage;
    public float range;
    public damageType spellType;
    public float manaCost;
    public float cooldown;
    public float knockback;
    public float stunTime;
    public Texture2D abilityIcon;

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
