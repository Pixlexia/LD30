using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float speed = 5f;
	float jumpForce = 600;
	Vector3 move;
	// Use this for initialization
	void Start () {
		Vector3 move = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
		move = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);

		transform.position += move * speed * Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Z)) {
			transform.rigidbody2D.AddForce(new Vector2(0,jumpForce));		
		}
	
	}
}
