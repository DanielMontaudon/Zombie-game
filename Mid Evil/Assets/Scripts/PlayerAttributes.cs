using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 100f;
    public float stamina = 100f;


    float timeInterval = 0f;
    public PlayerMovement playerMovement;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RechargeStamina();
    }
    private void RechargeStamina()
    {
        timeInterval += Time.deltaTime;
        if (playerMovement.state == PlayerMovement.MovementState.walking || playerMovement.state == PlayerMovement.MovementState.crouching)
        {
            if (stamina < 20)
            {
                if (timeInterval >= .5f && stamina < 100)
                {
                    timeInterval = 0;
                    stamina += 1;
                }
            }
            else
            {
                if (timeInterval >= .1f && stamina < 100)
                {
                    timeInterval = 0;
                    stamina += 1;
                }
            }
        }
    }
}
