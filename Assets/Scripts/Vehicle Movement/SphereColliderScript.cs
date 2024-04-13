using UnityEngine;

public class SphereColliderScript : MonoBehaviour
{
    private CarController carController;

    void Start()
    {
        
        carController = GetComponentInParent<CarController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            // Destroys the power-up object
            Destroy(other.gameObject);

            
            carController.OnApplySpeedBoost(); 
        }
    }
}