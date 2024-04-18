using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject ObjectsPrefabs;
    [SerializeField] private Vector3 prefabOffset;
    
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage addedImage in args.added)
        {
            InstantiateObject(addedImage);
        }
        
        foreach (ARTrackedImage updatedImage in args.updated)
        {
            UpdateObject(updatedImage);
        }

        foreach (ARTrackedImage removedImage in args.removed)
        {
            Destroy(instantiatedObjects[removedImage.referenceImage.name]);
            instantiatedObjects.Remove(removedImage.referenceImage.name);
        }
    }

    private void InstantiateObject(ARTrackedImage image)
    {
        var newObject = Instantiate(ObjectsPrefabs, image.transform.position + prefabOffset, Quaternion.identity);
        instantiatedObjects.Add(image.referenceImage.name, newObject);
        UpdateObject(image); // Position and rotation update for initially added images
    }

    private void UpdateObject(ARTrackedImage image)
    {
        if (instantiatedObjects.TryGetValue(image.referenceImage.name, out GameObject existingObject))
        {
            existingObject.transform.position = image.transform.position + prefabOffset;
            existingObject.transform.rotation = image.transform.rotation;
        }
    }
}
