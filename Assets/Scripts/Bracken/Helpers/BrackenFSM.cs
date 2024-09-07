using Patterns;

public class BrackenFSM: FSM {
    public BrackenFSM(): base() {}

    public void Add(BrackenFSMState state) {
        m_states.Add((int)state.ID, state);
    }

    public BrackenFSMState GetState(BrackenFSMStateType key) {
        return (BrackenFSMState)GetState((int)key);
    }

    public void SetCurrentState(BrackenFSMStateType stateKey) {
        State state = m_states[(int)stateKey];
        if(state != null) {
            SetCurrentState(state);
        }
    }
}

public class BrackenFSMState: State {
    public BrackenFSMStateType ID { get { return _id; }}
    protected Bracken _bracken = null;
    protected BrackenFSMStateType _id;

    public BrackenFSMState(FSM fsm, Bracken bracken): base(fsm) {
        _bracken = bracken;
    }

    public BrackenFSMState(Bracken bracken): base(fsm: bracken.brackenFSM) {
        _bracken = bracken;
        m_fsm = _bracken.brackenFSM;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}