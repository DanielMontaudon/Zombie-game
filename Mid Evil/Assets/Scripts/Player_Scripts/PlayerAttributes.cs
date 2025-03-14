using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 100f;
    public float stamina = 100f;
    public float mana = 100f;


    float staminaTimeInterval = 0f;
    float manaTimeInterval = 0f;

    PlayerMovement playerMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("Mana: " + mana + ", Stamina: " + stamina);
        RechargeStamina();
        RechargeMana();
    }
    private void RechargeStamina()
    {
        staminaTimeInterval += Time.deltaTime;
        if (playerMovement.state == PlayerMovement.MovementState.walking || playerMovement.state == PlayerMovement.MovementState.crouching)
        {
            if (stamina < 20)
            {
                if (staminaTimeInterval >= .5f && stamina < 100)
                {
                    staminaTimeInterval = 0;
                    stamina += 1;
                }
            }
            else
            {
                if (staminaTimeInterval >= .1f && stamina < 100)
                {
                    staminaTimeInterval = 0;
                    stamina += 1;
                }
            }
        }
    }
    private void RechargeMana()
    {
        manaTimeInterval += Time.deltaTime;
        if (manaTimeInterval >= 1f && mana < 100)
        {
            manaTimeInterval = 0;
            mana += 1;
        }

    }

}
