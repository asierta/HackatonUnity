using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BehaviourSearch : ByTheTale.StateMachine.State
{
    public EnemyStateMachineBase enemyBase { get { return (EnemyStateMachineBase)machine; } }
    public float distTraveled { get; protected set; }
    public override void Enter()
    {
        base.Enter();

        distTraveled = 0;
    }

    public override void Execute()
    {
        base.Execute();

        distTraveled += Time.deltaTime * enemyBase.Speed;

        if (distTraveled >= enemyBase.iDistToPatrol)
        {
            enemyBase.ChangeState<BehaviourStopAndTurn>();
        }
        //MOVE CHAR
        //Get char rotation
        float dir = enemyBase.transform.rotation.eulerAngles.y;
        if (dir < 180) { enemyBase.MoveEnemy(Vector3.left * Time.deltaTime); }//enemyBase.GetComponent<CharacterController>().Move(Vector3.right * enemyBase.Speed * Time.deltaTime); }
        else { enemyBase.GetComponent<CharacterController>().Move(Vector3.right * enemyBase.Speed * Time.deltaTime); }

        //CAST RAY TO SEARCH FOR GUATAPITA, if MATCH, change state
        if (enemyBase.CastRayForwardToFindPlayer())
        {
            enemyBase.ChangeState<BehaviourAlerted>();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
        if (collision.collider.gameObject.tag != "Enemy")
        {
            distTraveled = enemyBase.iDistToPatrol;
        }
    }
}
