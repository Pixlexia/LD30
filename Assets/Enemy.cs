using UnityEngine;
using System.Collections;

public class Enemy : Attackable {
	public Animator anim;

	// movement
	public float dir; // -1 = player is in left of enemy, if 1 = player right of enemy
	public float distanceFromPlayer;

	// atk stats
	public bool canAtk;
	public float atkCounter, atkDelay;
	public float rangeAttemptAttack;

	public float speed;

	// Use this for initialization
	public override	void Start () {
		base.Start ();
		hp = 1;

		anim = GetComponent<Animator> ();

		// atk
		atkDelay = 1f;
		atkCounter = 0;
		canAtk = false;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		UpdateAttacks ();
		UpdateMovement ();
		UpdateAnimations ();
		CheckDistanceFromPlayer ();

		Physics2D.IgnoreLayerCollision(8, 11, (this.rigidbody2D.velocity.y > 0.0f)); 
	}

	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.name == "player") {
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.name == "player") {
		}
	}

	public virtual void UpdateAnimations(){

		if(anim != null)
			anim.SetInteger ("x", (int)rigidbody2D.velocity.x);

		int flip = (dir > 0) ? 1 : -1;

		if(this.gameObject.tag != "boss")
			transform.localScale = new Vector3(flip, 1, 1);

	}

	public virtual void UpdateMovement(){
		if (Player.EnemyCanActionPlayer()) {
			dir = GameObject.Find ("player").transform.position.x - transform.position.x;
		}
		else{
			rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
		}
	}

	public override void Hit (int damage)
	{
		base.Hit (damage);

		Knockback ();

	}

	void Knockback(){
		float knockbackDir = GameObject.Find ("player").transform.position.x - transform.position.x;

		float knock = (rigidbody2D.velocity.x == 0) ? 1500 : 1500;
		int mod = (knockbackDir > 0) ? -1 : 1;
		rigidbody2D.AddForce (new Vector2 (mod * knock, 0));	
	}

	public virtual void UpdateAttacks(){
		if (!canAtk) {
			atkCounter += Time.deltaTime;
			if(atkCounter > atkDelay){
				canAtk = true;
			}
		}
		else{
			Attack ();
		}
	}

	public virtual void Attack(){
		ResetAttackCounter ();
	}

	public virtual void ResetAttackCounter(){
		canAtk = false;
		atkCounter = 0;
	}

	public virtual void FollowPlayer(){
		float mod = (dir > 0) ? 1 : -1;

		rigidbody2D.AddRelativeForce(new Vector2(mod * speed, 0));
	}

	public virtual void CheckDistanceFromPlayer(){
		distanceFromPlayer = Vector3.Distance(transform.position, GameObject.Find ("player").transform.position);
	}
}
