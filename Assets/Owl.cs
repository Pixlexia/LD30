using UnityEngine;
using System.Collections;

public class Owl : Enemy {

	bool startFollowing, flyUp;
	float xDistanceFromPlayer, yDistanceFromPlayer;
	float flyUpSpeed;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		hp = 1;
		atkDelay = 1000;
		speed = 70;
		flyUpSpeed = 100;
		startFollowing = false;
		flyUp = false;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();


		GetDistanceFromPlayer ();

		if (Mathf.Abs (xDistanceFromPlayer) < 2) {
			startFollowing = true;		
		}

		if (Mathf.Abs (yDistanceFromPlayer) < 0.7f) {
			flyUp = true;		
		}
		else if (Mathf.Abs (yDistanceFromPlayer) > 4) {
			flyUp = false;
		}

	}

	void GetDistanceFromPlayer(){
		xDistanceFromPlayer = GameObject.Find ("player").transform.position.x - transform.position.x;
		yDistanceFromPlayer = GameObject.Find ("player").transform.position.y - transform.position.y;
	}

	public override void UpdateMovement(){
		base.UpdateMovement ();
		if (startFollowing) {
			FollowPlayer();		
		}

		if (flyUp) {
			rigidbody2D.AddRelativeForce(new Vector2(0, flyUpSpeed));
		}
	}

	public override void UpdateAnimations(){
		base.UpdateAnimations ();
		if (rigidbody2D.velocity.x != 0 || rigidbody2D.velocity.y != 0)
			anim.SetBool ("isMoving", true);
		else
			anim.SetBool ("isMoving", false);
	}
}