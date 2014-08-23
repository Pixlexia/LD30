using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// prefabs
	public Transform bullet;
	
	// movement stuff
	float speed = 8f;
	float jumpForce = 900;
	Vector3 move;
	Vector2 move2;
	bool facingRight;

	// Jump stuff
	bool onGround;

	// Attack stuff
	bool canAtk;
	float atkCounter; // used for attack delay/aspd
	float atkDelay; // delay between attacks in seconds
	public static int damage;

	// Use this for initialization
	void Start () {
		Vector3 move = new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);

		// initialize attack stats
		atkCounter = 0;
		atkDelay = 0.1f; // 0.15s in between atks
		damage = 1;
	}
	
	// Update is called once per frame
	void Update () {
//		Movement ();
//		Movement2 ();
		Movement3 ();
		AttackControls ();
	}

	void AttackControls(){
		// compute for attackspeed
		if (!canAtk) {
			atkCounter += Time.deltaTime;
			if(atkCounter >= atkDelay)
				canAtk = true;
		}

		// player canAtk and presses attack button
		if(canAtk && Input.GetKey (KeyCode.X)){
			Attack();
		}
	}

	void Attack(){
		LongRangeAttack ();

		// reset atkcounter and canAtk (used for aspd)
		atkCounter = 0;
		canAtk = false;
	}

	void LongRangeAttack(){
		float bulletDir = 0;
		
		if (!facingRight) {
			bulletDir = -180f;
		}
		Instantiate (bullet, transform.position, Quaternion.Euler(new Vector3(0,0,bulletDir)));
	}

	void Movement3(){
		if (Input.GetKey (KeyCode.LeftArrow)) {
			facingRight = false;
			rigidbody2D.velocity = new Vector2(-speed,rigidbody2D.velocity.y);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			facingRight = true;
			rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
		}
		else{
			rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
		}

		Jump ();
	}

	void Jump(){
		// check if on ground
		if(Mathf.Round (rigidbody2D.velocity.y) == 0){
			onGround = true;
		}
		else{
			onGround = false;
		}

		if (onGround && Input.GetKeyDown (KeyCode.Z)) {
			transform.rigidbody2D.AddForce (new Vector2 (0, jumpForce));		
		}
	}

	void Movement(){	
		if (Input.GetKeyDown (KeyCode.Z)) {
				transform.rigidbody2D.AddForce (new Vector2 (0, jumpForce));		
		}
	}

	void Movement2(){
		if (Input.GetKey (KeyCode.LeftArrow)) {
			move.x = -1;		
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			move.x = 1;
		}
		else{
			move.x = 0;
		}

		rigidbody2D.velocity = new Vector2(move.normalized.x * speed * Time.deltaTime, rigidbody2D.velocity.y);
	}
}	
