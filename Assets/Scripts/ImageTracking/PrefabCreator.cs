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
        public float verticalOffset;
        public PrefabType type; // Add this line
    }

    public enum PrefabType // Define this enum
    {
        Mesh,
        Terrain
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

                // Check if the instantiated object is a terrain or mesh
                if (item.type == PrefabType.Terrain)
                {
                    // Special handling for terrain, e.g., adjusting scale or components
                    AdjustTerrain(newObj);
                }
                else if (item.type == PrefabType.Mesh)
                {
                    // Handling for meshes, if any specific adjustments needed
                    AdjustMesh(newObj);
                }

                instantiatedObjects.Add(image.referenceImage.name, newObj);
                break;
            }
        }
    }

    private void AdjustTerrain(GameObject terrainObj)
    {
        // Adjustments specific to terrains, such as setting layer, collider adjustments, etc.
        terrainObj.layer = LayerMask.NameToLayer("TerrainLayer");
    }

    private void AdjustMesh(GameObject meshObj)
    {
        // Adjustments specific to mesh objects, such as material settings, etc.
        meshObj.GetComponent<MeshRenderer>().material.color = Color.white;
    }


    private void UpdateObject(ARTrackedImage image)
    {
        if (instantiatedObjects.TryGetValue(image.referenceImage.name, out GameObject obj))
        {
            Vector3 adjustedPosition = image.transform.position + Vector3.up * imagePrefabs.First(i => i.imageName == image.referenceImage.name).verticalOffset;
            obj.transform.position = adjustedPosition;

            ImageToPrefab item = imagePrefabs.First(i => i.imageName == image.referenceImage.name);
            if (item.type == PrefabType.Terrain)
            {
                // Additional terrain updates
            }
            else if (item.type == PrefabType.Mesh)
            {
                obj.transform.rotation = image.transform.rotation; // Typically, meshes might need rotation updates
            }
        }
    }


}
