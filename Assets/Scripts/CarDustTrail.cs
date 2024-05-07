using UnityEngine;

public class CarDustTrail : MonoBehaviour
{
    public ParticleSystem dustTrail;
    public Rigidbody carRigidbody;
    public float minimumSpeed = 1f;

    private void Update()
    {
        // Check if the car is moving
        if (carRigidbody.velocity.magnitude > minimumSpeed)
        {
            // If moving, play the dust trail
            if (!dustTrail.isPlaying)
            {
                dustTrail.Play();
            }
        }
        else
        {
            // If not moving, stop the dust trail
            if (dustTrail.isPlaying)
            {
                dustTrail.Stop();
            }
        }
    }
}
