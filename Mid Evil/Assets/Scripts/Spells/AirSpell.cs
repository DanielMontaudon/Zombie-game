using UnityEngine;

public class AirSpell : Spell
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 5;
        range = 20;
        spellType = damageType.Ray;
    }

    public void castSpell()
    {

    }
}
