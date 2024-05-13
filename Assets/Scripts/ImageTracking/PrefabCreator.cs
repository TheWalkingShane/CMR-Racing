using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabCreator : MonoBehaviour
{
    [System.Serializable]
    public struct ImageToPrefab
    {
        public string imageName;
        public GameObject prefab;
        public float verticalOffset;  // Added vertical offset
    }

    [SerializeField] private ImageToPrefab[] imagePrefabs;
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();

    private ARTrackedImageManager arTrackedImageManager;

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
        foreach (ARTrackedImage image in args.added)
        {
            InstantiateObject(image);
        }
        
        foreach (ARTrackedImage image in args.updated)
        {
            UpdateObject(image);
        }

        foreach (ARTrackedImage image in args.removed)
        {
            if (instantiatedObjects.TryGetValue(image.referenceImage.name, out GameObject obj))
            {
                Destroy(obj);
                instantiatedObjects.Remove(image.referenceImage.name);
            }
        }
    }

    private void InstantiateObject(ARTrackedImage image)
    {
        foreach (ImageToPrefab item in imagePrefabs)
        {
            if (item.imageName == image.referenceImage.name)
            {
                Vector3 adjustedPosition = image.transform.position + Vector3.up * item.verticalOffset;
                GameObject newObj = Instantiate(item.prefab, adjustedPosition, Quaternion.identity);
                instantiatedObjects.Add(image.referenceImage.name, newObj);
                break;
            }
        }
    }


    private void UpdateObject(ARTrackedImage image)
    {
        if (instantiatedObjects.TryGetValue(image.referenceImage.name, out GameObject obj))
        {
            Vector3 adjustedPosition = image.transform.position + Vector3.up * imagePrefabs.First(i => i.imageName == image.referenceImage.name).verticalOffset;
            obj.transform.position = adjustedPosition;
            obj.transform.rotation = image.transform.rotation;
        }
    }

}
