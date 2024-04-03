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
    
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public float boostMult = 2.0f;
    
    public LayerMask groundLayer;
    public Rigidbody sphereRB;

    public float airDrag;
    public float groundDrag;
    
    // joystick controls
    public Joystick joyStick;

    // buttons
    public myButton MyButton;
    public myButton boostButton;
    public float dampenPress = 0;
    public float buttonSensitivity = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        // detaches the sphere for the car
        sphereRB.transform.parent = null;
    }

    private void GetInput()
    {
        //moveInput = Input.GetAxisRaw("Vertical");
        //turnInput = Input.GetAxisRaw("Horizontal");

        moveInput = joyStick.Vertical;
        turnInput = joyStick.Horizontal;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
        // check if the button has been pressed, then increase the sensitivity
        if (MyButton.isPressed)
        {
            dampenPress += buttonSensitivity * Time.deltaTime;
        }
        else
        {
            dampenPress -= buttonSensitivity * Time.deltaTime;
        }
        dampenPress = Mathf.Clamp01(dampenPress);
        
        // Boost
        if (boostButton.isPressed)
        {
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }
        
        // if the move input is greater than 0, then multiple by the forward speed,
        // else multiple by the reverse speed. This adjusts the speed of the car
        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;
        
        if (isBoosting)
        {
            moveInput *= boostMult;
           // Debug.Log($"Boosted, boost Count: {moveInput}");
        }
        
        // sets the cars position to sphere
        transform.position = sphereRB.transform.position;
        
        // sets the cars rotation 
        float newRotation = turnInput * turnSpeed * Time.deltaTime * joyStick.Vertical;
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
}
