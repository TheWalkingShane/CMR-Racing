using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;
    
    public float forwardSpeed;
    public float reverseSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;
    
    public Rigidbody sphereRB;

    public float airDrag;
    public float groundDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        // detaches the sphere for the car
        sphereRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        
        // if the move input is greater than 0, then multiple by the forward speed,
        // else multiple by the reverse speed. This adjusts the speed of the car
        moveInput *= moveInput > 0 ? forwardSpeed : reverseSpeed;
        
        // sets the cars position to sphere
        transform.position = sphereRB.transform.position;
        
        // sets the cars rotation 
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
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
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            // adds extra gravity 
            sphereRB.AddForce(transform.up * -30);
        }
    }
}
