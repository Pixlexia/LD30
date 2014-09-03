using UnityEngine;
using System.Collections;

public class LevelPortal : MonoBehaviour {

	public int goTo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.name == "player") {
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				Application.LoadLevel ("boss" + goTo);
			}
		}
	}
}
