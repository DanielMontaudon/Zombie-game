using UnityEngine;

public class SpellLifetime : MonoBehaviour
{
    public float spellDuration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("CullVfx", spellDuration);
    }

    private void CullVfx()
    {
        Destroy(this.gameObject);
    }
}
