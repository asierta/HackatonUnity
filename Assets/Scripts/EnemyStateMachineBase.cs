using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyStateMachineBase : ByTheTale.StateMachine.MachineBehaviour
{
    // Start is called before the first frame update
    public float iDistToPatrol = 1.0f;
    public float Speed { get { return speed; } }
    public float TimeUntilTurn { get { return timeToTurn; } }
    public float ShootingFrequency { get { return shootRatio; } }
    public float BulletTotDist { get { return bulletLife * bulletSpeed; } }
    public float EnemyViewRange { get { return viewRange; } }
    public float BulletLife { get { return bulletLife; } }
    public float BulletSpeed { get { return bulletSpeed; } }
    public CharacterController enemyController;
    //public GameObject projectile;
    public Transform spawnPosOfProjectiles = null;

    private void Awake()
    {
        enemyController = gameObject.GetComponent<CharacterController>();
    }


    public override void AddStates()
    {
        AddState<BehaviourSearch>();
        AddState<BehaviourStopAndTurn>();
        AddState<BehaviourAlerted>();
        AddState<BehaviourAttack1>();

        SetInitialState<BehaviourSearch>();
    }

    public void MoveEnemy(Vector3 MovementVector)
    {
        gameObject.transform.position += (MovementVector * speed);
    }

    private Vector3 getDirection()
    {
        Vector3 direction = Vector3.left;
        if (gameObject.transform.rotation.eulerAngles.y >= 180) { direction *= -1; }
        return direction;
    }

    public void ShootProjectile()
    {
        GameObject proj = (GameObject)Instantiate(projectile, spawnPosOfProjectiles.position, spawnPosOfProjectiles.rotation);
        Vector3 direction = getDirection();
        proj.GetComponent<Rigidbody>().AddForce(bulletSpeed * direction);
        Destroy(proj, bulletLife);
    }

    public bool CastRayForwardToFindPlayer(out float distance)
    {
        Vector3 direction = getDirection();
        RaycastHit rayHit;
        bool doesItHit = Physics.Raycast(spawnPosOfProjectiles.position, direction, out rayHit, viewRange);
        if (doesItHit)
        {
            if (rayHit.collider.tag == "Player")
            {
                distance = Vector3.Distance(rayHit.point, gameObject.transform.position);
                return true;
            }
        }
        distance = -1.0f;
        return false;
    }
    public bool CastRayForwardToFindPlayer()
    {
        return CastRayForwardToFindPlayer(out _);
    }

    [SerializeField] protected float speed = 1.0F;
    [SerializeField] protected float timeToTurn = 0.1F;
    [SerializeField] protected float shootRatio = 2.0F;
    [SerializeField] protected float bulletLife = 1.0F;
    [SerializeField] protected float bulletSpeed = 3.0F;
    [SerializeField] protected float viewRange = 5.0F;
    [SerializeField] public GameObject projectile = null;
}
