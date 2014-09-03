using UnityEngine;
using System.Collections;

public class Boss : Enemy {

	public float currentStateCounter, currentStateTime;
	public int state, totalStates;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		hp = 20;
		state = 0;
		currentStateCounter = 0;
		currentStateTime = 1;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public virtual void StateTimer(){
		currentStateCounter += Time.deltaTime;
		
		if (currentStateCounter > currentStateTime)
			NextState ();
	}
	
	public virtual void NextState(){
		currentStateCounter = 0;
	}
}
