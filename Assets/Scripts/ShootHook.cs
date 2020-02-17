using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHook : MonoBehaviour
{
    public GameObject HookPrefab;
    public Transform PlayerTransform;
    public LineRenderer lineRender;
    public float ProjecileForce = 1;

    private bool canShoot = true;
    private GameObject instantiated = null;
    private void Start()
    {
        lineRender.positionCount = 2;
        lineRender.startWidth = 1;
        lineRender.endWidth = 1;
    }

    private void FixedUpdate()
    {
        if (canShoot && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
            canShoot = false;

            instantiated = Instantiate(HookPrefab, gameObject.transform);
            Vector3 force;
            force = gameObject.transform.position - PlayerTransform.position;
            force.Normalize();
            instantiated.GetComponentInChildren<Rigidbody>().AddForce(force * ProjecileForce);
        }
        else if (!canShoot && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Delete");
            canShoot = true;
            Destroy(instantiated);
            instantiated = null;
        }
        else if (!canShoot)
        {
            //Debug.Log(instantiated.GetComponentInChildren<Transform>().position);
            lineRender.SetPosition(0, PlayerTransform.position);
            lineRender.SetPosition(1, instantiated.GetComponentInChildren<Transform>().position);
        }
    }
}
