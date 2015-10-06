using UnityEngine;
using System.Collections;

/// <summary>
/// The role of the cerebrum is to choose which actions the actor will take.
/// </summary>
abstract class Cerebrum<T>
{
    protected StateMachine FSM;
    protected T actor;

    public AIState CurrentState { get { return FSM.CurrentState; } }

    public Cerebrum(GameObject actor)
    {
        FSM = new StateMachine(actor);
        this.actor = actor.GetComponent<T>();
    }

    /// <summary>
    /// Contains decision-making logic of the cerebrum.
    /// </summary>
    abstract public void Think();

    /// <summary>
    /// Starts a new state.
    /// </summary>
    public void React(AIState state)
    {
        FSM.SwitchState(state);
    }

    /// <summary>
    /// Remembers current state and starts a new one.
    /// </summary>
    public void ReactAndRemember(AIState state)
    {
        FSM.OverrideState(state);
    }

    /// <summary>
    /// Stops current state and continues with old states (if any).
    /// </summary>
    public void SignalStop()
    {
        FSM.DeleteState();
    }
}