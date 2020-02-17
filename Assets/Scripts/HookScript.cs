using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    private bool HasLanded = false;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);
    }
}
