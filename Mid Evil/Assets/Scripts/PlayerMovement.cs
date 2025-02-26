using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float x;
    private float z;

    Vector3 velocity;
    bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Makes a point on groundcheck object to see if collides with anything
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset velocity once on ground
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }
        
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * Time.deltaTime * speed);


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity of y = 1/2gravity * time^2
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
