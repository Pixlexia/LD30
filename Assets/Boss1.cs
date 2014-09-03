using UnityEngine;
using System.Collections;

public class Boss1 : Boss {

	public Transform owl, arrow;
	public float bulletRotations, bulletRotIncrement;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		totalStates = 5;

		float numOfBullets = 30;
		bulletRotIncrement = 360f / numOfBullets;
		bulletRotations = 0;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		StateTimer ();

//		Debug.Log (currentStateCounter + "/" + currentStateTime);

		// update anim
		if(state == 1)
			anim.SetBool ("isFlapping", true);
		else
			anim.SetBool ("isFlapping", false);
	}

	public override void StateTimer(){
		base.StateTimer ();
	}

	public override void UpdateMovement(){
		base.UpdateMovement ();

		switch (state) {
		case 1: // flapping
			FlappingState();
			break;

		case 2: // attacking
			break;
		}
	}

	public override void UpdateAttacks(){
		if (state == 2 || state == 4) {
			base.UpdateAttacks ();
		}
	}


	public override void Attack(){
		if(state == 2){
			atkDelay = 0.4f;

			// spawn owl
			if(GameObject.FindGameObjectsWithTag("enemy").Length < 3)
				Instantiate (owl, transform.position + new Vector3(Random.Range (-1,2) * 3f, Random.Range (0,2) + 4f, 0), Quaternion.identity);
		}
		if (state == 4) {
			atkDelay = 0.02f;
			Instantiate(arrow, transform.position, Quaternion.Euler(new Vector3(0,0, bulletRotations + Random.Range (-2,2))));
			bulletRotations -= bulletRotIncrement;
		}
		base.Attack ();
	}

	void FlappingState(){
		if(Player.EnemyCanActionPlayer())
			GameObject.Find("player").gameObject.rigidbody2D.AddRelativeForce(new Vector2(dir * 20, 0));
	}

	public override void NextState(){
		base.NextState ();

		state++;
		if (state > totalStates)
			state = 0;

		switch (state) {
		case 0: currentStateTime = 0; break; // idle		
		case 1: currentStateTime = 3; break; // flap
		case 2: currentStateTime = 1.2f; break; // summon owls
		case 3: currentStateTime = 0; break; // idle
		case 4: currentStateTime = 4; break; // arrows
		}

		Debug.Log (state);
	}

	public override void BulletHit(Collider2D col){
		Bullet bullet = col.gameObject.GetComponent<Bullet> ();
		if (bullet != null) {
			// take damage
			Hit (bullet.damage);
			
			// instantiate particle
			if(sparkParticle != null)
				Instantiate (sparkParticle, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);
			
			// destroy bullet
			Destroy (bullet.gameObject);
		}
	}


	public override void Hit(int damage){
		base.Hit (damage);
	}
}
