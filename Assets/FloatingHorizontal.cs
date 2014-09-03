using UnityEngine;
using System.Collections;

public class FloatingHorizontal : MonoBehaviour {
	public bool floatup;
	public Vector3 temp,temp2;
	public float range,hovertime;
	// Use this for initialization
	void Start () {
		range = 0.4f;
		hovertime = 2;
		temp = transform.position;
		temp2 = transform.position;
		floatup = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(floatup)
			StartCoroutine(floatingup());
		else if(!floatup)
			StartCoroutine(floatingdown());
	}

	public IEnumerator floatingup(){
		temp.y = temp.y + range * Time.deltaTime;
		transform.position = temp;
		yield return new WaitForSeconds(hovertime);
		floatup = false;
	}
	public IEnumerator floatingdown(){
		temp.y -= range * Time.deltaTime;
		transform.position = temp;
		yield return new WaitForSeconds(hovertime);
		floatup = true;
	}
}