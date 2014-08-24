using UnityEngine;
using System.Collections;

public class Slash : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (KillOnAnimationEnd ());
	}

	private IEnumerator KillOnAnimationEnd() {
		yield return new WaitForSeconds (0.33f/2f);
		Destroy (this.gameObject);
	}
}
