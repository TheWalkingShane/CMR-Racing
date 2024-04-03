using UnityEngine;

public class AI_StatesController : MonoBehaviour
{
    public enum State { Accelerating, Reversing, Boosting, Turning, Idle }
    public State currentState;

    public float forwardSpeed;
    public float reverseSpeed;
    public float boostMult = 2.0f;
    public float turnSpeed;
    private bool isBoosting = false;

    public Rigidbody sphereRB; // The AI's Rigidbody
    public Transform[] waypoints; // Array of waypoints for the AI to follow
    private int currentWaypointIndex = 0; // Index of the current waypoint

    public float groundDrag;
    public float airDrag;

    void Update()
    {
        switch (currentState)
        {
            case State.Accelerating:
                if (ShouldTurn())
                {
                    ChangeState(State.Turning);
                }
                break;
            case State.Turning:
                if (!ShouldTurn())
                {
                    ChangeState(State.Accelerating);
                }
                break;
            case State.Reversing:
                Reverse();
                break;
            case State.Boosting:
                Boost();
                break;
            case State.Idle:
                // Possible implementation for Idle state
                break;
        }

        // Example transition (you would have more complex logic)
        if (currentState == State.Accelerating && ShouldTurn())
        {
            ChangeState(State.Turning);
        }
    }

    void ChangeState(State newState)
    {
        currentState = newState;
        // Initialize state-specific settings if needed
    }

    void Accelerate()
    {
        if (IsGrounded())
        {
            sphereRB.AddForce(transform.forward * forwardSpeed, ForceMode.Acceleration);
        }
    }

    void Reverse()
    {
        if (IsGrounded())
        {
            sphereRB.AddForce(-transform.forward * reverseSpeed, ForceMode.Acceleration);
        }
    }

    void Boost()
    {
        if (IsGrounded() && !isBoosting)
        {
            isBoosting = true;
        }
    }

    void Turn()
    {
        Vector3 toWaypoint = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        float turnAmount = Vector3.Cross(transform.forward, toWaypoint).y;
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    bool ShouldTurn()
    {
        // Implement logic to determine if the AI should start turning
        // For example, based on the angle to the next waypoint
        return false; // Placeholder
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            sphereRB.drag = groundDrag;
            Accelerate();
            Turn();
        }
        else
        {
            sphereRB.drag = airDrag;
            // Apply any airborne logic or adjustments
        }
    }

    bool IsGrounded()
    {
        // Implement grounding check similar to player's script
        return true; // Placeholder
    }
}