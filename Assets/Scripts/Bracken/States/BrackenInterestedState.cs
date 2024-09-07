using UnityEngine;

public class BrackenInterestedState: BrackenFSMState {
    private readonly int patrolRadius = 2;
    private readonly int playerDetectionRadius = 10;
    private readonly int moveSpeed = 2;
    private float lookTime = 0f; // Timer to track how long players are looking
    private bool isBeingLookedAt = false; // Flag to determine if any player is looking
    private const float timeThreshold = 2f; // Threshold to determine the next Bracken state
    private Transform currentTarget;

    public BrackenInterestedState(Bracken bracken): base(bracken) {
        _id = BrackenFSMStateType.INTERESTED;
    }

    public override void Enter() {
        base.Enter();
        _bracken.pathController.EnableRotation(false);
        _bracken.pathController.SetMoveSpeed(moveSpeed);
        _bracken.pathController.SetRandomDestination(patrolRadius);
        _bracken.pathController.OnTargetReachedEvent += OnTargetReached;

        // Choose random player as target
        PlayerController target = ChooseRandomPlayerAsTarget();
        if(target != null) {
            currentTarget = target.gameObject.transform;
        }
    }

    public override void Update() {
        base.Update();
        if(currentTarget != null) {
            _bracken.transform.LookAt(currentTarget); // Stare at target for intimidation
        }

        // Check if any players are looking at the Bracken
        var players = _bracken.playerDetector.GetPlayersWithinRadius(playerDetectionRadius);
        if(_bracken.playerDetector.IsAnyoneLookingAtMe(players)) {
            // Start the timer if this is the first time someone is looking at the Bracken
            if(!isBeingLookedAt) {
                isBeingLookedAt = true;
                lookTime = 0f;
            }
            lookTime += Time.deltaTime;

            // Check if any player(s) are staring for too long
            if(IsStaringTooLong(lookTime)) {
                // Set state to AGGRESSIVE
                _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.AGGRESSIVE);
            }
        } else if (isBeingLookedAt) {
            // Stop the timer and evaluate how long the Bracken was stared at for
            isBeingLookedAt = false;

            // If stare is too long, set state to AGGRESSIVE
            if(IsStaringTooLong(lookTime)) {
                // Set state to AGGRESSIVE
                _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.AGGRESSIVE);
            } else { // else Bracken should run away
                // Set state to SHY
                _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.SHY);
            }
            lookTime = 0f;
        }

        // If no players are nearby, exit this state and start patrolling
        if(players.Count == 0) {
            _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.PATROLLING);
        }
    }

    public override void Exit() {
        base.Exit();
        _bracken.pathController.OnTargetReachedEvent -= OnTargetReached;
        currentTarget = null;
        lookTime = 0f;
        isBeingLookedAt = false;
    }

    private bool IsStaringTooLong(float time) {
        if(time > 0f && time <= timeThreshold) {
            return false;
        } else if (time > timeThreshold) {
            return true;
        }
        return false;
    }

    private void OnTargetReached() {
        _bracken.pathController.SetRandomDestination(patrolRadius);
    }

    private PlayerController ChooseRandomPlayerAsTarget() {
        var players = _bracken.playerDetector.GetPlayersWithinRadius(playerDetectionRadius);
        if(players.Count == 0) return null;
        int randomIndex = Random.Range(0, players.Count);
        return players[randomIndex];
    }

}