using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class ResourceBar : MonoBehaviour
{
    [Header("Resource Bars")]
    [SerializeField] private Image healthbarImage;
    [SerializeField] private Image staminabarImage;
    [SerializeField] private Image manabarImage;

    [Header("Ability Bar")]
    [SerializeField] private Image UltimateImage;
    [SerializeField] private Image DefensiveImage;
    [SerializeField] private Image TornadoImage;
    [SerializeField] private Image DashImage;

    [SerializeField] private Image UltimateCDImage;
    [SerializeField] private Image DefensiveCDImage;
    [SerializeField] private Image TornadoCDImage;
    [SerializeField] private Image DashCDImage;

    [SerializeField] private TMP_Text UltCost;
    [SerializeField] private TMP_Text DefCost;
    [SerializeField] private TMP_Text TorCost;
    [SerializeField] private TMP_Text DashCost;


    float ultTimer;
    float defTimer;
    float torTimer;
    float dashTimer;

    private PlayerAttributes pa;
    private PlayerCombat pc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pa = gameObject.GetComponentInParent<PlayerAttributes>();    
        pc = gameObject.GetComponentInParent<PlayerCombat>();

        UltimateImage.sprite = pc.specialSpell.abilityIcon;
        DefensiveImage.sprite = pc.defensiveSpell.abilityIcon;
        TornadoImage.sprite = pc.rightSpell.abilityIcon;
        DashImage.sprite = pc.dashSpell.abilityIcon;

        UltCost.text = pc.specialSpell.manaCost.ToString();
        DefCost.text = pc.defensiveSpell.manaCost.ToString();
        TorCost.text = pc.rightSpell.manaCost.ToString();
        DashCost.text = pc.dashSpell.manaCost.ToString();

        ultTimer = pc.specialSpell.cooldown;
        defTimer = pc.defensiveSpell.cooldown;
        torTimer = pc.rightSpell.cooldown;
        dashTimer = pc.dashSpell.cooldown;


    }

    // Update is called once per frame
    void Update()
    {
        healthbarImage.fillAmount =  Mathf.Lerp(healthbarImage.fillAmount, pa.health / 100f, Time.deltaTime * 10f);
        staminabarImage.fillAmount = Mathf.Lerp(staminabarImage.fillAmount, pa.stamina / 100f, Time.deltaTime * 10f);
        manabarImage.fillAmount = Mathf.Lerp(manabarImage.fillAmount, pa.mana / 100f, Time.deltaTime * 10f);

        if(!pc.specialOffCooldown)
        {
            UltCooldown();
        }
        else
        {
            ultTimer = pc.specialSpell.cooldown;
            UltimateCDImage.fillAmount = 0f;
        }

        if (!pc.defensiveOffCooldown)
        {
            DefCooldown();
        }
        else
        {
            defTimer = pc.defensiveSpell.cooldown;
            DefensiveCDImage.fillAmount = 0f;
        }

        if (!pc.rightOffCooldown)
        {
            TorCooldown();
        }
        else
        {
            torTimer = pc.rightSpell.cooldown;
            TornadoCDImage.fillAmount = 0f;
        }

        if (!pc.dashOffCooldown)
        {
            DashCooldown();
        }
        else
        {
            dashTimer = pc.dashSpell.cooldown;
            DashCDImage.fillAmount = 0f;
        }

    }

    private void UltCooldown()
    {

        if (ultTimer > 0)
        {
            ultTimer -= Time.deltaTime;
            UltimateCDImage.fillAmount = ultTimer / pc.specialSpell.cooldown;
        }
    }

    private void DefCooldown()
    {
        if (defTimer > 0)
        {
            defTimer -= Time.deltaTime;
            DefensiveCDImage.fillAmount = defTimer / pc.defensiveSpell.cooldown;
        }
    }

    private void TorCooldown()
    {
        if (torTimer > 0)
        {
            torTimer -= Time.deltaTime;
            TornadoCDImage.fillAmount = torTimer / pc.rightSpell.cooldown;
        }
    }

    private void DashCooldown()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            DashCDImage.fillAmount = dashTimer / pc.dashSpell.cooldown;
        }
    }
    /*
    private IEnumerator cooldownWheel(float timer, Image sprite)
    {
        float fillValue = timer;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            sprite.fillAmount = timer / fillValue;


        }
    }
    */
}
