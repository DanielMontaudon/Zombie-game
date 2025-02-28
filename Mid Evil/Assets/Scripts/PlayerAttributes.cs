using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 100f;
    public float stamina = 100f;


    float timeInterval = 0f;
    public PlayerMovement characterMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //RechargeStamina();
    }
    private void RechargeStamina()
    {
        timeInterval += Time.deltaTime;
        if (timeInterval >= .1f && stamina < 100 && !characterMovement.isSprinting)
        {
            timeInterval = 0;
            stamina += 1;
        }
    }
}
