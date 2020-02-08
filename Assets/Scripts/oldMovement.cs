using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldMovement : MonoBehaviour
{
    public float strength = 50;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddForce(transform.forward * strength, ForceMode.VelocityChange); // mass does not affect
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddForce((transform.forward*-1) * strength, ForceMode.VelocityChange); // mass does not affect
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
