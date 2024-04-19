using System.Collections.Generic;
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
                GameObject newObj = Instantiate(item.prefab, image.transform.position, image.transform.rotation);
                instantiatedObjects.Add(image.referenceImage.name, newObj);
                break;
            }
        }
    }

    private void UpdateObject(ARTrackedImage image)
    {
        if (instantiatedObjects.TryGetValue(image.referenceImage.name, out GameObject obj))
        {
            obj.transform.position = image.transform.position;
            obj.transform.rotation = image.transform.rotation;
        }
    }
}
