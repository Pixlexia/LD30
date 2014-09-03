using UnityEngine;
using System.Collections;

public class BlinkingSprite : MonoBehaviour {
	public float blinkCounter, blinkTime; // blinking animations when invul
	public bool blinking;
	
	// Use this for initialization
	void Start () {
		blinking = false;

		blinkCounter = 0;
		blinkTime = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
		if (blinking) {
			blinkCounter += Time.deltaTime;

			if(blinkCounter > blinkTime){
				bool enabled = GetComponent<SpriteRenderer>().enabled;
				GetComponent<SpriteRenderer>().enabled = (enabled)? false : true;
				blinkCounter = 0;
			}
		}
	}
}
