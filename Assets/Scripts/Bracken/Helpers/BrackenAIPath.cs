using Pathfinding;
using System;

public class BrackenAIPath: AIPath {
    public Action OnTargetReachedEvent;

    public override void OnTargetReached() {
        base.OnTargetReached();
        OnTargetReachedEvent?.Invoke();
    }
}
