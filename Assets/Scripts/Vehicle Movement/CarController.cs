using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    private bool isBoosting = false;
    private bool isDrifting = false;
    private float boostForce;

    public float forwardSpeed;
    public float reverseSpeed;
    public float boostMult = 2.0f;
    public float driftForce = 10f;
    public float turnSpeed;
    public LayerMask groundLayer;

    public Rigidbody sphereRB;

    public float airDrag;
    public float groundDrag;

    public bool canMove = true; // Flag to control car movement

    
    void Start()
    {
        // Detach the sphere for the car
        sphereRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return; // Exit update if car cannot move

        // Input handling
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        // Boost
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isBoosting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBoosting = false;
        }

        // Drift
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDrifting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isDrifting = false;
        }

        // Drift adjustments
        if (isDrifting)
        {
            moveInput *= 0.5f;  // Reduce forward/backward movement during drift
            turnInput *= 2f;    // Increase turn speed during drift
        }

        // Calculate movement speed based on input
        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;

        // Applies boost multiplier
        if (isBoosting)
        {
            moveInput *= boostMult;
        }

        // Set car's position to sphere's position
        transform.position = sphereRB.transform.position;

        // Rotate the car
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);

        // Check for ground
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // Rotate the car to be parallel with the ground
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        // Adjust drag based on ground state
        if (isCarGrounded)
        {
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }
    }

    // FixedUpdate is called every fixed frame-rate frame
    void FixedUpdate()
    {
        if (canMove && isCarGrounded)
        {
            // Apply forward movement force
            float boostFactor = isBoosting ? boostMult : 1f;
            float totalMoveInput = moveInput * boostFactor;
            sphereRB.AddForce(transform.forward * totalMoveInput, ForceMode.Acceleration);

            // Apply drifting force
            if (isDrifting)
            {
                Vector3 driftForceVector = transform.right * (turnInput * driftForce);
                sphereRB.AddForce(driftForceVector, ForceMode.Acceleration);
            }
        }
        else
        {
            // Apply extra gravity when not grounded
            sphereRB.AddForce(transform.up * -30f);
        }
    }

    // Method to apply speed boost
    public void OnApplySpeedBoost()
    {
        forwardSpeed *= 1.25f; 
        
        StartCoroutine(ResetSpeedBoost(5f)); // Resets boost after 5 seconds 
    }

    
    private IEnumerator ResetSpeedBoost(float delay)
    {
        yield return new WaitForSeconds(delay);
        forwardSpeed /= 1.25f; // Restores the original forward speed
    }
}