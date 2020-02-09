using UnityEngine;
using System.Collections;

public class AddForceScript : MonoBehaviour {

	public float strength = 50;
	// Use this for initialization

	Rigidbody rigidBody;
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidBody.AddForce(transform.forward*strength, ForceMode.Acceleration); // mass does not affect
//		rigidBody.AddForce(transform.forward*strength, ForceMode.Force); // mass affects
//		if (Input.GetKey(KeyCode.F)) {
//			rigidBody.AddForce(transform.forward*strength, ForceMode.Impulse); // mass affects
//		}
//		if (Input.GetKey(KeyCode.F)) {
//			rigidBody.AddForce(transform.forward*strength, ForceMode.VelocityChange); // mass does not affect
//		}
	}
}
