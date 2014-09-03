using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour {
	public static float shake = 0;
	float shakeAmount = 0.2f;
	float decreaseFactor = 0.8f;
	public static bool shaking;
	public static Vector3 camAftershake;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision (8,9);
		Physics2D.IgnoreLayerCollision (9,9);
		Physics2D.IgnoreLayerCollision (8,8);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateShake ();
	}

	public void UpdateShake(){
		if(shaking){
			if(shake > 0){
				GameObject.Find ("Camera").camera.transform.position += Random.insideUnitSphere * shakeAmount;
				shake -= Time.deltaTime * decreaseFactor;
			}
			else{ // shake ends
				shake = 0.0f;
				shaking = false;
			}
		}
	}

	public static void Shake(float amount){
		camAftershake = GameObject.Find ("Camera").camera.transform.position;
		shake = amount;
		shaking = true;
	}
}
