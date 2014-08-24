using UnityEngine;
using System.Collections;

public class Mino : Enemy {

	// Use this for initialization
	public override void Start () {
		base.Start ();

		hp = 6;
		speed = 80;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		if(Player.EnemyCanActionPlayer())
			FollowPlayer ();
	}

	public void FollowPlayer(){
		float mod = (dir > 0) ? 1 : -1;

		rigidbody2D.AddRelativeForce(new Vector2(mod * speed, 0));
	}
}