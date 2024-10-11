using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTourismManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager; // Manages raycasting for AR interactions
    public GameObject culturalSitePrefab;   // The 3D model or cultural site prefab
    
    private GameObject placedObject;        // Tracks the placed cultural site
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>(); // Cached list of hits

    // Ensures the ARRaycastManager is assigned
    void Awake()
    {
        if (raycastManager == null)
        {
            raycastManager = GetComponent<ARRaycastManager>();
            if (raycastManager == null)
            {
                Debug.LogError("ARRaycastManager component missing.");
                enabled = false; // Disable script if the component is missing
            }
        }
    }

    // Handles touch input and AR placement
    void Update()
    {
        if (Input.touchCount == 0 || culturalSitePrefab == null) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                if (placedObject == null)
                {
                    // Instantiate cultural site at hit position
                    placedObject = Instantiate(culturalSitePrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    // Update existing object position
                    placedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}
