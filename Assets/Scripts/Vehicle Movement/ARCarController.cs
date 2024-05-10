using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    // Start is called before the first frame update
    void Start()
    {
        // detaches the sphere for the car
        sphereRB.transform.parent = null;
    }

    private void GetInput()
    {
        moveInput = joyStick.Vertical * verticalSensitivity;
        turnInput = joyStick.Horizontal * horizontalSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
        // Boost
        if (boostButton.isPressed)
        {
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }
        
        // Drift
        if (driftButton.isPressed)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }
         
        // Adjustes the move and turn input when drifting
        if (isDrifting)
        {
            // Reduce forward/backward movement during drift
            moveInput *= 0.5f;
             
            // Increase turn speed during drift
            turnInput *= 2f;
        }
        
        // if the move input is greater than 0, then multiple by the forward speed,
        // else multiple by the reverse speed. This adjusts the speed of the car
        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;
        
        if (isBoosting)
        {
            moveInput *= boostMult;
        }
        
        // sets the cars position to sphere
        transform.position = sphereRB.transform.position;
        
        // sets the cars rotation 
        float newRotation = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);
        
        // check if there is ground
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        
        // rotate the car to be parallel with the ground
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        if (isCarGrounded)
        {
            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        if (isCarGrounded)
        {
            // If boosting, apply additional force
            boostForce = isBoosting ? forwardSpeed * boostMult : 0f;
            sphereRB.AddForce(transform.forward * (moveInput + boostForce), ForceMode.Acceleration);
            
            // If drifting, applying the additional sideways force
            if (isDrifting)
            {
                Vector3 driftForceVector = transform.right * (turnInput * driftForce);
                sphereRB.AddForce(driftForceVector, ForceMode.Acceleration);
            }
        }
        else
        {
            // adds extra gravity 
            sphereRB.AddForce(transform.up * -30);
        }
    }
}
