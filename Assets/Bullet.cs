using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float range;
	public float speed = 30f;
	public int damage;
	public Transform wallSpark;
	public Vector3 initialPos; // bullets starting position, used for range

	// Use this for initialization
	public virtual void Start () {
		initialPos = transform.position;
		damage = Player.damage;
		Destroy (gameObject, 20);
		rigidbody2D.velocity = transform.right * speed;
		range = 20;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		CalculateRange ();
	}

	void CalculateRange(){
		if (Vector3.Distance (initialPos, transform.position) > range) {
			Destroy (this.gameObject);		
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "obstacle") {
			// create spark particle
			Instantiate (wallSpark, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
}
