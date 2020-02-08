using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourStopAndTurn : ByTheTale.StateMachine.State
{
    // Start is called before the first frame update
    public EnemyStateMachineBase enemyBase { get { return (EnemyStateMachineBase)machine; } }
    public float timeInState { get; protected set; }

    public override void Enter()
    {
        base.Enter();
        timeInState = 0;
    }

    public override void Execute()
    {
        base.Execute();
        timeInState += Time.deltaTime;

        if (timeInState >= enemyBase.TimeUntilTurn)
        {
            enemyBase.transform.Rotate(0.0f, 180.0f, 0.0f);
            machine.ChangeState<BehaviourSearch>();
        }
    }
}
