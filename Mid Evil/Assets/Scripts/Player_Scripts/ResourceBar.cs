using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Image healthbarImage;
    [SerializeField] private Image staminabarImage;
    [SerializeField] private Image manabarImage;


    private PlayerAttributes pa;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pa = gameObject.GetComponentInParent<PlayerAttributes>();    
    }

    // Update is called once per frame
    void Update()
    {
        healthbarImage.fillAmount =  Mathf.Lerp(healthbarImage.fillAmount, pa.health / 100f, Time.deltaTime * 10f);
        staminabarImage.fillAmount = Mathf.Lerp(staminabarImage.fillAmount, pa.stamina / 100f, Time.deltaTime * 10f);
        manabarImage.fillAmount = Mathf.Lerp(manabarImage.fillAmount, pa.mana / 100f, Time.deltaTime * 10f);

    }
}
