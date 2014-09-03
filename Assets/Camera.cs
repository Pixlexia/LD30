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

		float mod = 0.5f;

		if (Application.loadedLevelName == "boss2")
			mod = -2f;
		else if(Application.loadedLevelName == "boss1")
			mod = 0;

		transform.position = Vector3.Lerp (transform.position, new Vector3(playerpos.x, playerpos.y + mod, transform.position.z), 
		                                   Time.deltaTime * smooth);
	}
	
	void Update () {
	
	}
}
