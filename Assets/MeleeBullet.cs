using UnityEngine;
using System.Collections;

public class MeleeBullet : Bullet {

	// Use this for initialization
	public override void Start () {
		base.Start ();	
		range = 1.2f;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}
}
