using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldMovement : MonoBehaviour
{
    public float speed = 10;
    new Rigidbody rigidbody;
    Animator myAnimator;


    //To check if it is touching the floor
    float distanceToground;
    public bool isGrounded = false;




    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        myAnimator = this.GetComponent<Animator>();


        distanceToground = GetComponent<Collider>().bounds.extents.y;
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * (rigidbody.mass + 2));
        //rigidbody.velocity = inputVector*2;       

        if (!Physics.Raycast(transform.position, -Vector3.up, distanceToground + 0.3f))
        {
            isGrounded = false;
            print("On the air");
            myAnimator.SetBool("isGrounded", false);
        }
        else
        {
            isGrounded = true;
            print("Ob thle floor");
            myAnimator.SetBool("isGrounded", true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        myAnimator.SetFloat("distanceToTarget", horizontal);

        //print(myAnimator.GetFloat("distanceToTarget"));
        Vector3 movement = new Vector3(horizontal* speed *Time.deltaTime, 0, vertical * speed *Time.deltaTime);
        rigidbody.MovePosition(transform.position + movement);


        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(transform.up * 11.2f, ForceMode.Impulse); // mass affects
            print("Holi soy el salto");
        }

    }




}
