using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        // Rotate towards the current waypoint
        RotateTowardsWaypoint();

        // Move towards the current waypoint
        MoveTowardsWaypoint();
    }

    void RotateTowardsWaypoint()
    {
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    void MoveTowardsWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

        // Check if the bot has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Reset to the first waypoint if reached the end
                currentWaypointIndex = 0;
            }
        }
    }
}