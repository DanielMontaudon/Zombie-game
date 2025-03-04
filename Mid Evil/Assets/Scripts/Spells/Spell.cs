using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Scriptable Objects/Spell")]
public class Spell : ScriptableObject
{
    public float damage;
    public float range;
    public damageType spellType;
    public float manaCost;
    public float cooldown;

    //Help Denote for ray type
    //Lightning - Ray
    //Air - Ray
    //Fire - Sphere
    //Earth - 
    public enum damageType
    {
        Lightning,
        Air,
        Fire,
        Earth
    }

}
