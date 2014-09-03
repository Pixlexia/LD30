using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour {
	public Transform sparkParticle, dieParticle;
	
	public int hp;	
	public bool isFlash;
	public float flashCounter;

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		UpdateFlash ();
	}

	void UpdateFlash(){
		if (isFlash) {
			flashCounter += Time.deltaTime;
			if(flashCounter > 0.08f){
				isFlash = false;
				flashCounter = 0;
			}
		}

		if (isFlash) {
			// flash sprite
			GetComponent<SpriteRenderer> ().color = Color.red;
		} 
		else {
			// normal
			GetComponent<SpriteRenderer>( ).color = Color.white;
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "playerbullet")
			BulletHit (col);
	}

	// hit by player melee or ranged
	public virtual void BulletHit(Collider2D col){
		Bullet bullet = col.gameObject.GetComponent<Bullet> ();
		if (bullet != null) {
			// take damage
			Hit (bullet.damage);

			// instantiate particle
			if(sparkParticle != null)
				Instantiate (sparkParticle, transform.position, Quaternion.identity);

			// destroy bullet
			Destroy (bullet.gameObject);
		}
	}

	public virtual void Hit(int damage){
		hp -= damage;

		if (hp <= 0) {
			Die ();
		}

		isFlash = true;
	}

	public virtual void Die(){
		if (dieParticle != null) {
			Instantiate (dieParticle, transform.position, Quaternion.identity);
			Debug.Log ("enemy die");
		}

		Destroy (this.gameObject);
	}
}
