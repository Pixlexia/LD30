using UnityEngine;
using System.Collections;

public class BossPortal : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectsWithTag ("boss").Length == 0) {
			GetComponent<SpriteRenderer> ().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;
		}
	}
	
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.name == "player") {
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				Application.LoadLevel ("1");
			}
		}
	}
}
