using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDetector: MonoBehaviour {
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private BoxCollider boxCollider;

    /// <summary>
    /// Returns a list of PlayerControllers that are within the passed radius
    /// </summary>
    /// <returns></returns>
    public List<PlayerController> GetPlayersWithinRadius(int radius) {
        Collider[] hitColliders = Physics.OverlapBox(
            gameObject.transform.position,
            transform.localScale * radius, // The size of our radius
            Quaternion.identity,
            playerLayer
        );
        return hitColliders.ToList().ConvertAll(a => a.GetComponent<PlayerController>());
    }

    // Returns true if any players in the list are looking at this gameObject
    // otherwise, returns false
    public bool IsAnyoneLookingAtMe(List<PlayerController> players) {
        for(int i = 0; i < players.Count; i++) {
            if(players[i].TryGetComponent(out PlayerController possibleTarget)) {
                if(IsGameObjectInView(possibleTarget.playerCamera)) {
                    return true;
                }
            }
        }
        return false;
    }

    // Returns whether the current gameObject is within view of the given Camera
    private bool IsGameObjectInView(Camera cam) {
        // Check if the object is within camera bounds
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Bounds bounds = boxCollider.bounds;
        if(!GeometryUtility.TestPlanesAABB(planes, bounds)) {
            return false;
        }

        // Check if the object is visible within the camera (not occluded)
        Vector3[] corners = new Vector3[8];
        bounds.GetCorners(corners); 

        foreach(Vector3 corner in corners) {
            Vector3 direction = corner - cam.transform.position;
            if(Physics.Raycast(cam.transform.position, direction, out RaycastHit hit)) {
                if(hit.collider.gameObject == gameObject) {
                    return true;
                }
            }
        }
        return false;
    }
}