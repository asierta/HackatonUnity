using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourAttack1 : ByTheTale.StateMachine.State
{
    public EnemyStateMachineBase enemyBase { get { return (EnemyStateMachineBase)machine; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        enemyBase.ShootProjectile();
    }
}
