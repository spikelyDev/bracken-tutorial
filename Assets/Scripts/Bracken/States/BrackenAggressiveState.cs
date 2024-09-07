using UnityEngine;

public class BrackenAggressiveState: BrackenFSMState {
    private readonly float moveSpeed = 6f;
    private readonly int radius = 20;
    private Transform currentTarget;

    public BrackenAggressiveState(Bracken bracken): base(bracken) {
        _id = BrackenFSMStateType.AGGRESSIVE;
    }

    public override void Enter() {
        base.Enter();
        _bracken.pathController.EnableRotation(true);
        _bracken.pathController.SetMoveSpeed(moveSpeed);
        _bracken.pathController.OnTargetReachedEvent += OnTargetReached;

        // Choose a random player to target and attack
        var players = _bracken.playerDetector.GetPlayersWithinRadius(radius);
        if(players.Count > 0) {
            int randomIndex = Random.Range(0, players.Count);
            currentTarget = players[randomIndex].transform;
        } else {
            // Set state to SHY if no players are nearby
            _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.SHY);
        }
    }

    public override void Update() {
        base.Update();

        // If no players nearby, exit state
        if(_bracken.playerDetector.GetPlayersWithinRadius(radius).Count == 0) {
            _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.SHY);
        }
    }

    public override void Exit() {
        base.Exit();
        _bracken.pathController.OnTargetReachedEvent -= OnTargetReached;
        currentTarget = null;
    }

    private void OnTargetReached() {
        // Just keep chasing target 
        if(currentTarget != null) {
            _bracken.pathController.SetDestination(currentTarget.position);
        }
    }
}