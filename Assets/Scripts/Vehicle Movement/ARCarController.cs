using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARCarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;
    private bool isBoosting = false;
    private float boostForce;
    
    public float forwardSpeed;
    public float reverseSpeed;
    public float boostMult = 2.0f;
    public float turnSpeed;
    public LayerMask groundLayer;
    
    public Rigidbody sphereRB;

    public float airDrag;
    public float groundDrag;

    // UI buttons for car controls
    public Button accelerateButton;
    public Button reverseButton;
    public Button boostButton;
    public Button leftButton;
    public Button rightButton;

    // Start is called before the first frame update
    void Start()
    {
        // detaches the sphere for the car
        sphereRB.transform.parent = null;

        // Add onClick listeners to buttons
        accelerateButton.onClick.AddListener(Accelerate);
        reverseButton.onClick.AddListener(Reverse);
        boostButton.onClick.AddListener(ToggleBoost);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    // Update is called once per frame
    void Update()
    {
        // No need to get input from keys or joystick

        // sets the cars position to sphere
        transform.position = sphereRB.transform.position;
        
        // sets the cars rotation 
        float newRotation = turnInput * turnSpeed * Time.deltaTime * moveInput;
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
        }
        else
        {
            // adds extra gravity 
            sphereRB.AddForce(transform.up * -30);
        }
    }

    // Button click event methods
    void Accelerate()
    {
        moveInput = forwardSpeed;
    }

    void Reverse()
    {
        moveInput = -reverseSpeed;
    }

    void ToggleBoost()
    {
        isBoosting = !isBoosting;
    }

    void MoveLeft()
    {
        turnInput = -1f;
    }

    void MoveRight()
    {
        turnInput = 1f;
    }
}
