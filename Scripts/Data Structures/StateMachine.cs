using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class StateMachine
{
    private Stack<AIState> stateStack = new Stack<AIState>();
    private GameObject actor;

    public StateMachine(GameObject actor)
    {
        this.actor = actor;
    }

    /// <summary>
    /// Forgets the old state and starts a new one.
    /// </summary>
    public void SwitchState(AIState state)
    {
        if (CurrentState != null)
            stateStack.Pop().OnStop(actor);

        stateStack.Push(state);
        state.OnStart(actor);
    }

    /// <summary>
    /// Starts a new state without removing the current one from the stack.
    /// This means that if the current state is deleted, the FSM returns
    /// to the previous state.
    /// </summary>
    public void OverrideState(AIState state)
    {
        CurrentState.OnStop(actor);
        stateStack.Push(state);
        state.OnStart(actor);
    }

    /// <summary>
    /// Stops and forgets the current state, starting any state 
    /// that is still left on the FSM stack.
    /// </summary>
    public void DeleteState()
    {
        stateStack.Pop().OnStop(actor);

        if (CurrentState != null)
            CurrentState.OnStart(actor);
    }

    public AIState CurrentState
    {
        get
        {
            if (stateStack.Count > 0)
                return stateStack.Peek();
            else
                return null;
        }
    }
}
