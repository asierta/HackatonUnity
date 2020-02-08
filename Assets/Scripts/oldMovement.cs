using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldMovement : MonoBehaviour
{
    public float speed = 10;
    Rigidbody rigidbody;
    Animator myAnimator;

    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        myAnimator = this.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        myAnimator.SetFloat("distanceToTarget", horizontal);

        print(myAnimator.GetFloat("distanceToTarget"));
        Vector3 movement = new Vector3(horizontal* speed *Time.deltaTime, 0, vertical * speed *Time.deltaTime);
        rigidbody.MovePosition(transform.position + movement);


        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(transform.up * 9.5f, ForceMode.Impulse); // mass affects
            print("Holi soy el salto");
        }

    }



    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * (rigidbody.mass + 1));
        //rigidbody.velocity = inputVector*2;       
    }


}
