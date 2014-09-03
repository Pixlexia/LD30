using UnityEngine;
using System.Collections;

public class LeftWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Mathf.Lerp (transform.position.x, -6, 0.1f), 0, 0);
	}
}
