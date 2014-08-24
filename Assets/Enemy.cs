using UnityEngine;
using System.Collections;

public class Enemy : Attackable {
	public Animator anim;

	// movement
	public float dir;

	// atk stats
	public bool canAtk;
	public float atkCounter, atkDelay;

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
	}

	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.name == "player") {
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.name == "player") {
		}
	}

	void UpdateAnimations(){

		anim.SetInteger ("x", (int)rigidbody2D.velocity.x);

		int flip = (dir > 0) ? 1 : -1;

		transform.localScale = new Vector3 ((float) flip, 1f, 1f);

	}

	public virtual void UpdateMovement(){
		if (Player.EnemyCanActionPlayer()) {
			dir = GameObject.Find ("player").transform.position.x - transform.position.x;
		}
		else{
			rigidbody2D.velocity = new Vector2 (0, 0);
		}

	}

	public override void Hit (int damage)
	{
		base.Hit (damage);

		Knockback ();

	}

	void Knockback(){
		float dir = GameObject.Find ("player").transform.position.x - transform.position.x;

		float knock = (rigidbody2D.velocity.x == 0) ? 1500 : 1500;
		int mod = (dir > 0) ? -1 : 1;
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
	}
}
