using UnityEngine;
using System.Collections;

public class Archer : Enemy {
	public Transform bullet;


	// Use this for initialization
	public override void Start () {
		base.Start ();
		hp = 5000;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Attack(){
//		Instantiate (bullet, transform.position, Quaternion.identity);
//		canAtk = false;
//		atkCounter = 0;
	}
}
