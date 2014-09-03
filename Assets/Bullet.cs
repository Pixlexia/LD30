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
			Die();
		}
	}

	public virtual void Die(){

		Destroy (this.gameObject);		
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "obstacle") {
			// create spark particle
			Die ();
			Instantiate (wallSpark, transform.position, Quaternion.identity);
		}
		else if(col.gameObject.tag == "enemy"){
			Debug.Log ("hit enm");
		}	
		else if(col.gameObject.name == "player"){
			// Player.cs will destroy this bullet
		}
	}
}
