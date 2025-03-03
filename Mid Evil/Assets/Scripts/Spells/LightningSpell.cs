using UnityEngine;

public class LightningSpell : Spell
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = 10;
        range = 25;
        spellType = damageType.Ray;
    }

    public void castSpell()
    {

    }
}
