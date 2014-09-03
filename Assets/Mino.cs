using UnityEngine;
using System.Collections;

public class Mino : Enemy {

	// Use this for initialization
	public override void Start () {
		base.Start ();

		hp = 3;
		speed = 100;

		rangeAttemptAttack = 8;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if(distanceFromPlayer < rangeAttemptAttack && Player.EnemyCanActionPlayer())
			FollowPlayer ();
	}
}