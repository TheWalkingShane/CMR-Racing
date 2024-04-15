using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public float spinSpeed = 90.0f; 
    public float floatAmount = 0.1f;

    private Vector3 startPos; 

    void Start()
    {
        
        startPos = transform.position;
    }

    void Update()
    {
        // Apply spinning rotation
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

        
        Vector3 floatOffset = Vector3.up * Mathf.Sin(Time.time) * floatAmount;
        transform.position = startPos + floatOffset;
    }
}
