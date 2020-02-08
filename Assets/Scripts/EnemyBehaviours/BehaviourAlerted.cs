using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourAlerted : ByTheTale.StateMachine.State
{
    public EnemyStateMachineBase enemyBase { get { return (EnemyStateMachineBase)machine; } }
    public float timeInThisState { get; protected set; }

    public override void Enter()
    {
        base.Enter();
        timeInThisState = 0;
    }

    public override void Execute()
    {
        base.Execute();

        timeInThisState += Time.deltaTime;

        if (timeInThisState >= enemyBase.ShootingFrequency)
        {
            enemyBase.ChangeState<BehaviourAttack1>();
        }

        //Check if Guatapita is in front of you, if not, go back to patroling
        float dist;
        if (!enemyBase.CastRayForwardToFindPlayer(out dist))
        {
            enemyBase.ChangeState<BehaviourSearch>();
        }

        //If not close enough for the bullet to hit guatapita, get closer
        if (dist > enemyBase.BulletTotDist)
        {
            float dir = enemyBase.transform.rotation.eulerAngles.y;
            if (dir < 180) { enemyBase.MoveEnemy(Vector3.left * Time.deltaTime); }
            else { enemyBase.GetComponent<CharacterController>().Move(Vector3.right * enemyBase.Speed * Time.deltaTime); }
        }
    }
}
