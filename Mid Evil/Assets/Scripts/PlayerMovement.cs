using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    float timeInterval = 0f;
    float setMoveSpeed;
    PlayerAttributes stats;

    public bool isSprinting = false;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        setMoveSpeed = moveSpeed;

        stats = GetComponent<PlayerAttributes>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

    }

    //Take input from wasd
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            //lets you continue to jump if key is held down
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        print(stats.stamina);
        if (Input.GetKey(sprintKey) && grounded && stats.stamina > 0)
        {
            isSprinting = true;
            moveSpeed = setMoveSpeed * 2f;
            DrainStamina(1f);
            //print(isSprinting);
        }
        else
        {
            moveSpeed = setMoveSpeed;
            isSprinting = false;
            //print(isSprinting);
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed,
        // if faster than moveSpeed, apply new velocity to rigidBody
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity to always jump to same height
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //Impulse only applies once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Sprint()
    {

    }

    private void DrainStamina(float stamCost)
    {
        timeInterval += Time.deltaTime;
        if (timeInterval >= .1f)
        {
            timeInterval = 0;
            stats.stamina -= stamCost;
        }
    }
}
