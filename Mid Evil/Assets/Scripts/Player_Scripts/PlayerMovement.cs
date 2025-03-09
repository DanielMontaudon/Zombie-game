using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    private RaycastHit crouchHit;
    bool roofAbove;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
        


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    //My stuff
    float timeInterval = 0f;
    PlayerAttributes stats;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        stats = GetComponent<PlayerAttributes>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void Update()
    {
        //ground check
        grounded = Physics.CheckSphere(transform.position, 0.3f, whatIsGround);

        //roof check
        roofAbove = Physics.Raycast(transform.position, Vector3.up, playerHeight + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0.5f;

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

        //start crouch
        if(Input.GetKey(crouchKey))
        {
            Vector3 crouchScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, Time.deltaTime * 10f);
            //transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        //stop crouch
        if(!Input.GetKey(crouchKey) && !roofAbove)
        {
            Vector3 uncrouchScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            transform.localScale = Vector3.Lerp(transform.localScale, uncrouchScale, Time.deltaTime * 10f);
            //transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        //Mode - Crouching (pressing key or forced into crouch)
        if((Input.GetKey(crouchKey) && grounded) || (roofAbove && grounded))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            //rb.AddForce(Vector3.down * 0.5f, ForceMode.Impulse);
        }
        //Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey) && !roofAbove && stats.stamina > 5f)
        {
            DrainStamina(5f);
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        //Mode - Walking
        else if(grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        //Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopemoveDirection() * moveSpeed * 10f, ForceMode.Force);

            //if (rb.linearVelocity.y > 0)
                //rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            rb.AddForce(Vector3.down * 1.5f, ForceMode.Impulse);
        }

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();

    }

    private void SpeedControl()
    {
        if (OnSlope())
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        //limiting spped on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed,
            // if faster than moveSpeed, apply new velocity to rigidBody
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
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

    private bool OnSlope()
    {
        //Shoot a raycast down from middle of player, down to the floor (half player hight) and a little more 
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.3f))
        {
            //How steep the slope player is standing on is
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //print(angle);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopemoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    //Drains stamina at >= float seconds
    private void DrainStamina(float stamCost)
    {
        timeInterval += Time.deltaTime;
        if (timeInterval >= .2f)
        {
            timeInterval = 0;
            stats.stamina -= stamCost;

            if (stats.stamina < 0)
                stats.stamina = 0;
        }
    }


    private void OnDrawGizmos()
    {

        //roofAbove Ray
        Debug.DrawRay(transform.position, Vector3.up * playerHeight);

        //grounded Ray
        //Debug.DrawRay(transform.position, Vector3.down * 0.3f);

        Gizmos.color = new Color(1f,0f,1f,0.3f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
