using UnityEngine;

public class Spell : MonoBehaviour
{

    public float damage;
    public float range;
    public damageType spellType;
    public enum damageType
    {
        Ray,
        CheckSphere
    }
}
