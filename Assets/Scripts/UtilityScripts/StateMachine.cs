public class StateMachine<T>
{
    public State<T> currentState { get; private set; }
    public T Owner;

    public StateMachine(T _owner)
    {
        Owner = _owner;
        currentState = null;
    }

    public void ChangeState(State<T> _newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(Owner);
        }

        currentState = _newState;
        currentState.EnterState(Owner);
    }

    public void Update()
    {
        if (currentState != null)
            currentState.UpdateState(Owner);
    }

    public void FixedUpdate()
    {
        if (currentState != null)
            currentState.FixedUpdateState(Owner);
    }
}