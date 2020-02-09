using UnityEngine;
using System.Collections;

public class HaltPhysics : MonoBehaviour {

	bool done = false;
	public float distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray detectFloorRay = new Ray (transform.position, Vector3.down);

		Debug.DrawRay(transform.position, Vector3.down);

		if (!done) {
			if (Physics.Raycast(detectFloorRay, out hit, distance)) {
				done = true;
				ChangePhysics();
			}
		}
	}

	void ChangePhysics() {
		GetComponent<Rigidbody>().drag = 10;
	}
}
