using UnityEngine;

public class ARCarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private float boostForce;
    
    private bool isCarGrounded;
    private bool isBoosting = false;
    private bool isDrifting = false;
    
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public float boostMult = 2.0f;
    public float driftForce = 10f;
    
    public LayerMask groundLayer;
    public Rigidbody sphereRB;

    public float airDrag;
    public float groundDrag;
    
    // joystick controls
    public float horizontalSensitivity = 1.0f;
    public float verticalSensitivity = 1.0f;
    public Joystick joyStick;

    // buttons
    public myButton boostButton;
    public myButton driftButton;

    // CanMove property to control movement based on the countdown
    public bool CanMove { get; set; } = true;

    void Start()
    {
        sphereRB.transform.parent = null; // Detaches the sphere for independent physics simulation
    }

    void Update()
    {
        if (!CanMove) return; // Prevents input handling when CanMove is false

        GetInput();

        // Boost logic
        isBoosting = boostButton.isPressed;
        
        // Drift logic
        isDrifting = driftButton.isPressed;
         
        // Adjust input for drift
        if (isDrifting)
        {
            moveInput *= 0.5f; // Reduce forward/backward movement during drift
            turnInput *= 2f;   // Increase turn speed during drift
        }

        // Calculate movement input based on direction
        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;
        
        // Apply boost multiplier
        if (isBoosting)
        {
            moveInput *= boostMult;
        }

        // Update car's position to match the physics sphere's position
        transform.position = sphereRB.transform.position;
        
        // Update car's rotation
        float newRotation = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);

        // Ground check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        
        // Rotate the car to align with the ground normal
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        // Set drag based on whether the car is grounded
        sphereRB.drag = isCarGrounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        if (!CanMove) return; // Prevents physics updates when CanMove is false

        if (isCarGrounded)
        {
            // Apply main driving force
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);

            // Apply boosting force
            boostForce = isBoosting ? forwardSpeed * boostMult : 0;
            sphereRB.AddForce(transform.forward * boostForce, ForceMode.Acceleration);

            // Apply drift force
            if (isDrifting)
            {
                Vector3 driftForceVector = transform.right * (turnInput * driftForce);
                sphereRB.AddForce(driftForceVector, ForceMode.Acceleration);
            }
        }
        else
        {
            // Apply extra gravity force when not grounded
            sphereRB.AddForce(transform.up * -30);
        }
    }

    private void GetInput()
    {
        moveInput = joyStick.Vertical * verticalSensitivity;
        turnInput = joyStick.Horizontal * horizontalSensitivity;
    }
}