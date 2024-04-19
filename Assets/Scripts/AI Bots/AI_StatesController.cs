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
    public Transform nextWaypoint; // You need to define how the AI chooses or is assigned the next waypoint

    public float groundDrag;
    public float airDrag;
    
    void Update()
    {
        switch (currentState)
        {
            case State.Accelerating:
                if (ShouldTurn()) // You need to define ShouldTurn based on your AI's pathfinding logic
                {
                    ChangeState(State.Turning);
                }
                /*else if (ShouldBoost()) // Define ShouldBoost based on your game's boost logic
                {
                    ChangeState(State.Boosting);
                }*/
                break;
            case State.Turning:
                if (!ShouldTurn())
                {
                    ChangeState(State.Accelerating);
                }
                break;
            case State.Reversing:
                Reverse();
                // Transition logic can be added here
                break;
            case State.Boosting:
                Boost();
                // Transition logic can be added here
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
        // Apply forward speed; similar logic to the player's Accelerate
        // You need to adjust this to match AI behavior, possibly considering the direction to the next waypoint
        // Assuming forwardSpeed is already set based on your game's requirements
        if (IsGrounded())
        {
            sphereRB.AddForce(transform.forward * forwardSpeed, ForceMode.Acceleration);
        }
    }

    void Reverse()
    {
        // Apply reverse speed; similar logic to player's Reverse
        // Use reverseSpeed to move backwards
        if (IsGrounded())
        {
            sphereRB.AddForce(-transform.forward * reverseSpeed, ForceMode.Acceleration);
        }
    }

    void Boost()
    {
        // Apply boost; similar logic to the player's Boost
        if (IsGrounded() && !isBoosting)
        {
            isBoosting = true; // This flag could be reset elsewhere, depending on your boost logic (e.g., after a time delay or when a boost "resource" is depleted)
            // Boost logic can be more complex depending on whether you have a boost duration, cooldown, etc.
        }
    }

    /*void Turn()
    {
        // Calculate turn direction and apply turning logic
        // Similar to how you're applying turnInput in the player's script but based on AI logic towards next waypoint
        
        void Turn()
        {
            Vector3 toWaypoint = (nextWaypoint.position - transform.position).normalized;
            float turnAmount = Vector3.Cross(transform.forward, toWaypoint).y;

            // Adjusting the turn speed based on how directly the AI is facing the waypoint
            // You might need to scale turnAmount by your turnSpeed and potentially adjust based on deltaTime
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

    }
    */
    // Helper method to determine if AI should turn
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
            // Apply movement force if needed, similar to player's FixedUpdate logic
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
