using UnityEngine;
using System.Collections;


public class ArcanePuddle : MonoBehaviour
{
    public GameObject hazardArea;
    public float hazardActiveTime = 5f;
    HazardAreaLogic hal;

    private void Start()
    {
        hal = hazardArea.GetComponent<HazardAreaLogic>();
    }

    public void LightningArcane(float damage)
    {
        hal.damageFromSpell = damage / 2f;
        StartCoroutine(DeactivateHazard());
    }

    private IEnumerator DeactivateHazard()
    {
        hal.activated = true;

        yield return new WaitForSeconds(hazardActiveTime);

        hal.activated = false;
    }
}
