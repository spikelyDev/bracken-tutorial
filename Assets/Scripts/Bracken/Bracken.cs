using UnityEngine;

[RequireComponent(typeof(BrackenPathController))]
[RequireComponent(typeof(PlayerDetector))]
public class Bracken : MonoBehaviour {
    public BrackenFSM brackenFSM;
    public BrackenPathController pathController;
    public PlayerDetector playerDetector;

    private void Awake() {
        // Get component references
        pathController = GetComponent<BrackenPathController>();
        playerDetector = GetComponent<PlayerDetector>();
    }

    private void Start() {
        brackenFSM = new();

        // Add possible states
        brackenFSM.Add(new BrackenPatrollingState(this));
        brackenFSM.Add(new BrackenInterestedState(this));
        brackenFSM.Add(new BrackenShyState(this));
        brackenFSM.Add(new BrackenAggressiveState(this));

        // Set current state
        brackenFSM.SetCurrentState(BrackenFSMStateType.PATROLLING);
    }

    private void Update() {
        brackenFSM.Update();
    }

    private void FixedUpdate() {
        brackenFSM.FixedUpdate();
    }
}