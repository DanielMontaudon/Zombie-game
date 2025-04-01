using UnityEngine;
using System.Collections;


public class ArcanePuddle : MonoBehaviour
{
    public GameObject hazardArea;
    bool currentlyActive = false;
    public float hazardActiveTime = 5f;
    HazardAreaLogic hal;

    private void Start()
    {
        hal = hazardArea.GetComponent<HazardAreaLogic>();
    }

    public void LightningArcane(float damage)
    {
        if (!currentlyActive)
        {
            currentlyActive = true;
            hal.damageFromSpell = damage / 2f;
            hal.physicalEffect.GetComponent<MeshRenderer>().material = hal.lightningEffect;
            hal.physicalEffect.SetActive(true);
            StartCoroutine(DeactivateHazard());
        }
    }

    public void FireArcane(float damage)
    {
        if (!currentlyActive)
        {
            currentlyActive = true;
            hal.damageFromSpell = damage / 2f;
            hal.physicalEffect.GetComponent<MeshRenderer>().material = hal.fireEffect;
            hal.physicalEffect.SetActive(true);
            StartCoroutine(DeactivateHazard());
        }
    }



    private IEnumerator DeactivateHazard()
    {
        hal.activated = true;

        yield return new WaitForSeconds(hazardActiveTime);

        hal.physicalEffect.SetActive(false);
        hal.activated = false;
        currentlyActive = false;

    }
}
