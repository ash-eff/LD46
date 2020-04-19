using UnityEngine;

public class WaveController : MonoBehaviour
{
    public StateMachine<WaveController> stateMachine;
    public static WaveController waveController;

    private ObjectPooler pool;

    private void Awake()
    {
        waveController = this;
        stateMachine = new StateMachine<WaveController>(waveController);
        stateMachine.ChangeState(WCWaitState.Instance);
        pool = FindObjectOfType<ObjectPooler>();
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();
}
