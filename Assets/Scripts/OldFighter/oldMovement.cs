using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldMovement : MonoBehaviour
{
    public float speed;
    new Rigidbody rigidbody;
    Animator myAnimator;

    public GameObject floorCheck;

    [Header("Movement Axis")]
    public string horizontalAxis;
    public string verticalAxis;

    public KeyCode jumpKey;





    private Vector3 inputVector;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        myAnimator = this.GetComponent<Animator>();


      
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Physics.gravity * (rigidbody.mass + 2));

        if (this.GetComponent<oldFighter>().isDead())
        {
            return;
        }

        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);

        myAnimator.SetFloat("distanceToTarget", horizontal);

        //print(myAnimator.GetFloat("distanceToTarget"));
        Vector3 movement = new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
        rigidbody.MovePosition(transform.position + movement);


        if (Input.GetKeyDown(jumpKey))
        {

            if (floorCheck.GetComponent<FloorCheck>().isGrounded == true)
            {
                rigidbody.AddForce(transform.up * 11.2f, ForceMode.Impulse); // mass affects
                print("Holi soy el salto");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {    
    }
}
