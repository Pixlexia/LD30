using UnityEngine;
using System.Collections;

public class Enemy : Attackable {

	// Use this for initialization
	public override	void Start () {
		base.Start ();
		hp = 10;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Hit (int damage)
	{
		base.Hit (damage);
	}
}
