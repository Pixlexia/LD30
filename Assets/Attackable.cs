using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour {
	public Transform sparkParticle, dieParticle;
	
	public int hp;	

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

	public virtual void OnTriggerEnter2D(Collider2D col){
		BulletHit (col);


		Debug.Log ("hit");
	}

	void BulletHit(Collider2D col){
		Bullet bullet = col.gameObject.GetComponent<Bullet> ();
		if (bullet != null) {
			// take damage
			Hit (bullet.damage);

			// instantiate particle
			Instantiate (sparkParticle, bullet.gameObject.transform.position, Quaternion.identity);

			// destroy bullet
			Destroy (bullet.gameObject);
		}
	}

	public virtual void Hit(int damage){
		hp -= damage;

		if (hp <= 0) {
			Die ();
		}
	}

	public virtual void Die(){
		Destroy (this.gameObject);
	}
}
