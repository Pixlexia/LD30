using UnityEngine;
using System.Collections;

public class OwlBullet : Bullet {

	// Use this for initialization
	public override void Start () {
		speed = 4f + Random.Range (1,3);
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}
}
