using System;
using UnityEngine;

public class BrackenPathController : MonoBehaviour {
    [SerializeField] private BrackenAIPath aiPath;
    public Action OnTargetReachedEvent;

    private void Start() {
        aiPath.OnTargetReachedEvent += OnTargetReached;
    }

    private void OnTargetReached() {
        OnTargetReachedEvent?.Invoke();
    }

    private Vector3 PickRandomPoint(int radius) {
        var point = UnityEngine.Random.insideUnitSphere * radius;
        point.y = 0;
        point += aiPath.position;
        return point;
    }

    private Vector3 GetFurthestPointInOppositeDirection(Transform origin, int distance) {
        Vector3 oppositeDirection = -origin.forward;
        Vector3 furthestPoint = origin.position + (oppositeDirection * distance);
        furthestPoint.y = 0;
        return furthestPoint;
    }

    public void SetRandomDestination(int radius) {
        Vector3 randomDestination = PickRandomPoint(radius);
        SetDestination(randomDestination);
    }

    /// <summary>
    /// Sets the current target the Bracken should move towards
    /// </summary>
    /// <param name="target"></param>
    public void SetDestination(Vector3 target) {
        aiPath.destination = target;
        aiPath.SearchPath();
    }

    public void SetFurthestDestination(int radius) {
        Vector3 furthestDestination = GetFurthestPointInOppositeDirection(transform, radius);
        SetDestination(furthestDestination);
    }
    

    /// <summary>
    /// Sets the current move speed of the Bracken
    /// </summary>
    /// <param name="speed"></param>
    public void SetMoveSpeed(float speed) {
        aiPath.maxSpeed = speed;
    }

    public void EnableRotation(bool value) {
        aiPath.enableRotation = value;
    }
}
