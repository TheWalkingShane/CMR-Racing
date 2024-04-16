using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject ObjectsPrefabs;
    [SerializeField] private Vector3 prefabOffset;

    private GameObject Objecttype;
    private ARTrackedImageManager arTrackedImageManager;

    private void OnEnable()
    {
        arTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();

        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            Objecttype = Instantiate(ObjectsPrefabs, image.transform);
            Objecttype.transform.position += prefabOffset;
        }


    }
}
