public class BrackenShyState: BrackenFSMState {
    private readonly int radius = 20;
    private readonly float moveSpeed = 7f;
    public BrackenShyState(Bracken braken): base(braken) {
        _id = BrackenFSMStateType.SHY;
    }

    public override void Enter() {
        base.Enter();
        _bracken.pathController.EnableRotation(true);
        _bracken.pathController.SetMoveSpeed(moveSpeed);
        _bracken.pathController.SetFurthestDestination(radius);
        _bracken.pathController.OnTargetReachedEvent += OnTargetReached;
    }

    public override void Exit() {
        base.Exit();
        _bracken.pathController.OnTargetReachedEvent -= OnTargetReached;
    }

    private void OnTargetReached() {
        // Change state to patrolling once Bracken runs to furthest point
        _bracken.brackenFSM.SetCurrentState(BrackenFSMStateType.PATROLLING);
    }
}