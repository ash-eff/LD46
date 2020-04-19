public abstract class State<T>
{
    protected string stateName;
    public virtual State<T> createInstance() { return null; }
    public virtual string GetStateName() => stateName;
    public abstract void EnterState(T _owner);
    public abstract void ExitState(T _owner);
    public abstract void UpdateState(T _owner);
    public abstract void FixedUpdateState(T _owner);
}
