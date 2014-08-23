using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	float speed = 40f;
	public int damage;
	public Transform wallSpark;

	// Use this for initialization
	void Start () {
		damage = Player.damage;
		Destroy (gameObject, 20);
		rigidbody2D.velocity = transform.right * speed;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "obstacle") {
			// create spark particle
			Instantiate (wallSpark, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
}
