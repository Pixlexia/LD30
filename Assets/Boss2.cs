using UnityEngine;
using System.Collections;

public class Boss2 : Boss {

	public float bulletRotations, bulletRotIncrement;
	public Transform bullet;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		atkDelay = 0.5f;
		totalStates = 3;
		
		float numOfBullets = 10;
		bulletRotIncrement = 90f / numOfBullets;
		bulletRotations = 135f;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		StateTimer ();
	}
	
	public override void StateTimer(){
		base.StateTimer ();
	}
	
	public override void UpdateMovement(){
		base.UpdateMovement ();
		
		switch (state) {
		case 1:
			break;
			
		case 2:
			break;
		}
	}

	public override void UpdateAttacks(){
		if (state == 2) {
//			base.UpdateAttacks();		
		}
	}

	public override void Attack(){
		Debug.Log ("atk");
		Instantiate (bullet, transform.position, Quaternion.Euler (0,0,bulletRotations));
		bulletRotations -= bulletRotIncrement;
		base.Attack ();
	}
	
	public override void NextState(){
		base.NextState ();
		
		state++;
		if (state > totalStates)
			state = 0;
		
		switch (state) {
		case 0: currentStateTime = 1; break; // idle		
		case 1:
			bulletRotations = 135f;
			currentStateTime = 1; 
			break; // fire shit
		case 2: currentStateTime = 4; break; // idle
		}

		Debug.Log (state);
	}
	
	public override void Hit(int damage){
		base.Hit (damage);
	}
}
