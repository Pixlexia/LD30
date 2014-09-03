using UnityEngine;
using System.Collections;

public class Tentacle : MonoBehaviour {

	float stopAtY, defaultStopY;
	float initialY, initialX;
	float moveDelay, upCounter;
	bool goingUp, goingDown;
	float moveUpForce;
	int direction; // if odd, go down, if ieven go up

	// Use this for initialization
	void Start () {
		GetComponent<Animator> ().speed = Random.Range (0.75f, 1.25f);
		goingUp = true;
		goingDown = false;
		defaultStopY = -2;
		GetNewStopAtY ();
		initialY = transform.position.y;
		initialX = transform.position.x;
		upCounter = 0;
		moveDelay = Random.Range (0f, 3f);
		moveUpForce = 5f;

		direction = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();	
		
		if (GameObject.FindGameObjectsWithTag ("boss").Length == 0) {
			goingUp = false;
			goingDown = true;
		}
	}

	void Movement(){
		transform.position = new Vector2 (initialX, transform.position.y);

		if (goingUp){
			rigidbody2D.AddRelativeForce (new Vector2 (0, moveUpForce));
			if (transform.position.y > stopAtY) {
				goingUp = false;
				rigidbody2D.velocity = new Vector2(0,0);
			}
		}

		if(goingDown){
			rigidbody2D.AddRelativeForce (new Vector2 (0, -moveUpForce));
			if(transform.position.y < initialY){
				goingDown = false;
				rigidbody2D.velocity = new Vector2(0,0);
			}
		}
			
		upCounter += Time.deltaTime;
		if(upCounter > moveDelay){
			direction++;
			if(direction % 2 == 0)
				goingUp = true;
			else
				goingDown = true;

			upCounter = 0;
			moveDelay = Random.Range (0f,3f);

			if(goingDown)
				moveDelay = 2 + Random.Range(0f,3f);
		}
	}

	void GetNewStopAtY(){
		stopAtY = defaultStopY + Random.Range (0,2);
	}

	public void Die(){
	}
}
