using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForward : MonoBehaviour
{
    private float fSpeed = 0;
    Rigidbody rb;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>();
    }

    public void SetInformation(float lifetime, float speed, Vector3 forward)
    {
        Destroy(gameObject, lifetime);
        fSpeed = speed;
        rb.AddForce(fSpeed * forward);
    }
}
