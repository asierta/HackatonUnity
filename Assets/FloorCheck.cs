using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    Animator myAnimator;

    //To check if it is touching the floor
    float distanceToground;
    public bool isGrounded = false;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = transform.root.GetComponent<Animator>();
        distanceToground = transform.root.GetComponent<Collider>().bounds.extents.y;
    }


    void FixedUpdate()
    {
        RaycastHit hit;
        int layer_mask = LayerMask.GetMask("Suelo");
        Debug.DrawRay(transform.position, -Vector3.up * (distanceToground + 0.5f), Color.red, 2);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToground + 0.2f, layer_mask))
        {
            print(hit.transform.gameObject.layer);

            isGrounded = true;
            print("Ob thle floor");
            myAnimator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            print("On the air");
            myAnimator.SetBool("isGrounded", false);
        }

    }
}
