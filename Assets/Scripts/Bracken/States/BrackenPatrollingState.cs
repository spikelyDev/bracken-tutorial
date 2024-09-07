public class BrackenPatrollingState : BrackenFSMState
{
    private readonly int patrolRadius = 20;
    private readonly float moveSpeed = 6f;
    private readonly int playerDetectionRadius = 10;

    public BrackenPatrollingState(Bracken bracken) : base(bracken) {
        _id = BrackenFSMStateType.PATROLLING;
    }

    public override void Enter() {
        base.Enter();
        _bracken.pathController.EnableRotation(true);
        _bracken.pathController.SetMoveSpeed(moveSpeed);
        _bracken.pathController.SetRandomDestination(patrolRadius);
        _bracken.pathController.OnTargetReachedEvent += OnTargetReached;
    }

    public override void Update() {
        base.Update();

        // If a player is nearby, change the state to INTERESTED
        if(_bracken.playerDetector.GetPlayersWithinRadius(playerDetectionRadius).Count > 0) {
            _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.INTERESTED);
        }
    }

    public override void Exit() {
        base.Exit();
        _bracken.pathController.OnTargetReachedEvent -= OnTargetReached;
    }

    private void OnTargetReached() {
        _bracken.pathController.SetRandomDestination(patrolRadius);
    }
}