using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class QRCodeTracker : MonoBehaviour
{
    private ARTrackedImageManager _imageManager;

    private void Awake()
    {
        // Get the ARTrackedImageManager from the AR Session Origin
        _imageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        // Subscribe to the tracked images changed event
        if (_imageManager != null)
        {
            _imageManager.trackedImagesChanged += OnImageChanged;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the tracked images changed event
        if (_imageManager != null)
        {
            _imageManager.trackedImagesChanged -= OnImageChanged;
        }
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Iterate over the newly added tracked images
        foreach (var trackedImage in eventArgs.added)
        {
            // Check if the reference image name is "Map.one"
            if (trackedImage.referenceImage.name == "Map.one")
            {
                Debug.Log("Map.one QR Code Detected!");
                // Handle the event when the QR code is detected
                // For example, you can activate a UI element or start an animation
            }
        }
        
        // Optionally, you can handle updated and removed images as well
        foreach (var trackedImage in eventArgs.updated)
        {
            // Update handling if needed
        }
        
        foreach (var trackedImage in eventArgs.removed)
        {
            // Removal handling if needed
        }
    }
}