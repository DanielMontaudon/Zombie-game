using UnityEngine;

public class FireSpell : Spell
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 50;
        range = 10;
        spellType = damageType.CheckSphere;
    }

    public void castSpell()
    {

    }
}
