using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	float smooth = 8.0f;
	Vector2 playerpos, offset;
	
	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate(){
		playerpos = GameObject.Find ("player").gameObject.transform.position;
		transform.position = Vector3.Lerp (transform.position, new Vector3(playerpos.x, playerpos.y, transform.position.z), 
		                                   Time.deltaTime * smooth);
	}
	
	void Update () {
	
	}
}
