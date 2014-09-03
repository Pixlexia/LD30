using UnityEngine;
using System.Collections;

public class Archer : Enemy {
	public Transform bullet;


	// Use this for initialization
	public override void Start () {
		base.Start ();
		hp = 4;
		atkDelay = 2f;
		rangeAttemptAttack = 10f;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Attack(){
		if (distanceFromPlayer < rangeAttemptAttack && Player.EnemyCanActionPlayer()) {
			int dir = (GameObject.Find ("player").transform.position.x - transform.position.x > 0) ? 0 : -180;
			Instantiate (bullet, transform.position + new Vector3(0,Random.Range (0,0.3f),0), Quaternion.Euler(new Vector3(0,0,dir)));
			base.Attack ();
		}
	}
}