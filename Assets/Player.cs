using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// prefabs
	public Transform bullet;
	public Transform meleeAtk;
	public Transform slash1, slash2, slash3;

		
	// death and respawning stuff
	public enum LifeState { alive, knockback, deadOnGround, invul };
	public static LifeState state;
	public static bool isAlive;
	public float respawnCounter, respawnTime;
	public float invulCounter, invulTime;
	public float collideEnemyTime, dieTime;
	
	// sprites and anim
	public Animator anim;

	// movement stuff
	float speed = 7f;
	float jumpForce = 800;
	bool facingRight;

	// Physics stuff
	bool onGround;
	public bool canFly;
	public bool canSwim;

	// Attack stuff
	public bool isMelee;
	bool canAtk;
	bool isAttacking, isInCombo;
	float atkCounter; // used for attack delay/aspd
	float atkDelay; // delay between attacks in seconds
	int atkComboCount, maxCombo;
	float comboTimer, comboDelay, delayBetweenComboSet;
	public static int damage;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();

		// initialize attack stats
		atkCounter = 0;
		atkDelay = 0.1f; // secs in between atks
		atkComboCount = 0;
		maxCombo = 3; // combo = 3 atks
		comboDelay = 0.25f;
		delayBetweenComboSet = 1f;
		damage = 1;

		// death and respawning stuff
		isAlive = true;
		collideEnemyTime = 0; // time colliding with player
		dieTime = 0.1f; // player dies if enemy is colliding for this long
		state = LifeState.alive;

		respawnTime = 3; // time before player respawns from deadonground
		respawnCounter = 0;

		invulTime = 3; // time player is invulnerable upon respawning
		invulCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAlive || state == LifeState.invul){
			Movement ();
			AttackControls ();
		}

		if(!isAlive)
			Death ();

		UpdateAnimation ();

	}

	// called on update
	void AttackControls(){
		if (isMelee) {
			if(isInCombo){
				comboTimer += Time.deltaTime;

				if(comboTimer > comboDelay){
					EndCombo ();
				}
			}

			// compute for attackspeed
			if(!canAtk){
				atkCounter += Time.deltaTime;
				if(atkCounter >= atkDelay)
					canAtk = true;
			}

			// atk button
			if(canAtk && Input.GetKeyDown (KeyCode.X)){
				Attack();
			}
		} else {
			// compute for attackspeed
			if (!canAtk) {
				atkCounter += Time.deltaTime;
				if(atkCounter >= atkDelay)
					canAtk = true;
			}

			if(canAtk && Input.GetKey (KeyCode.X)){
				Attack ();
			}
		}

	}

	// called when player presses attack button
	void Attack(){
		// animation
		isAttacking = true;

		float bulletDir = 0;
		
		if (!facingRight) {
			bulletDir = -180f;
		}

		if (isMelee) {
			MeleeAttack (bulletDir);
		}
		else{	
			LongRangeAttack (bulletDir);
		}
	}

	void ResetAtkCounter(){
		// reset atkcounter and canAtk (used for aspd)
		atkCounter = 0;
		canAtk = false;
	}

	void ResetComboCounter(){
		
	}

	void MeleeAttack(float bulletDir){
		isInCombo = true;
		comboTimer = 0;


		Instantiate (meleeAtk, transform.position, Quaternion.Euler (new Vector3 (0, 0, bulletDir)));
		atkComboCount++;
		if (atkComboCount > maxCombo) {
			canAtk = false;
			EndCombo();
		}

		RenderSlashEffects (bulletDir);
		ResetAtkCounter ();
		UpdateAttackAnimation ();
	}

	void RenderSlashEffects(float bulletDir){
		int facing = (facingRight) ? 1 : -1;
		Quaternion q = Quaternion.Euler (new Vector3 (bulletDir, 0, bulletDir));

		switch (atkComboCount) {
		case 1:
			Instantiate (slash2, transform.position + new Vector3(facing * 0.5f,-0.1f,0), q);
			break;
			
		case 2:
			Instantiate (slash1, transform.position + new Vector3(facing * 0.25f,0.25f,0), q);
			break;
			
		case 3:
			Instantiate (slash3, transform.position + new Vector3(facing * 0.5f,0.1f,0), q);
			break;
		}
	}

	void EndCombo(){
		Debug.Log ("end combo");
		atkComboCount = 0;
		comboTimer = 0;
		isInCombo = false;
	}

	void LongRangeAttack(float bulletDir){
		Instantiate (bullet, transform.position, Quaternion.Euler(new Vector3(0,0,bulletDir)));
		ResetAtkCounter ();
	}

	void Movement(){
		if (Input.GetKey (KeyCode.LeftArrow)) {
			facingRight = false;
//			rigidbody2D.AddRelativeForce(new Vector2(speed,0));
			rigidbody2D.velocity = new Vector2(-speed,rigidbody2D.velocity.y);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			facingRight = true;
//			rigidbody2D.AddRelativeForce(new Vector2(speed,0));
			rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
		}
		else{
			rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
		}
		
		Jump ();		
		if(canFly) 
			FlyControls ();
		if(canSwim)
			SwimControls ();
	}

	void FlyControls(){
		if (Input.GetKey (KeyCode.Space)) {
			transform.rigidbody2D.AddRelativeForce(new Vector2(0, 20));		
		}
	}

	void SwimControls(){
	}

	void Death(){
		switch (state) {
		case LifeState.deadOnGround:
			respawnCounter += Time.deltaTime;
			if(respawnCounter > respawnTime){
				state = LifeState.invul;
				invulCounter = 0;
				GetComponent<BlinkingSprite>().blinking = true;
			}
			break;

		case LifeState.invul:
			invulCounter += Time.deltaTime;
			if(invulCounter > invulTime){
				state = LifeState.alive;
				isAlive = true;
				GetComponent<SpriteRenderer>().enabled = true;
				GetComponent<BlinkingSprite>().blinking = false;
			}
			break;

		default:
			break;
		}
	}

	void UpdateAttackAnimation(){
		anim.SetInteger ("atk", atkComboCount);
	}

	void UpdateMoveAnimation(){
		anim.SetInteger ("x", (int) rigidbody2D.velocity.x);
		
		if(facingRight){
			anim.transform.rotation = Quaternion.Euler(new Vector3(anim.transform.rotation.x, 0f, anim.transform.rotation.z));
		}
		else{
			anim.transform.rotation = Quaternion.Euler(new Vector3(anim.transform.rotation.x, -180f, anim.transform.rotation.z));
		}
	}

	void UpdateDeathAnimation(){
		anim.SetInteger ("death", (int) state);
	}

	void UpdateAnimation(){
		UpdateMoveAnimation ();
		UpdateAttackAnimation ();
		UpdateDeathAnimation ();
	}

	void Jump(){
		// check if on ground
		if(Mathf.Round (rigidbody2D.velocity.y) == 0){
			onGround = true;
		}
		else{
			onGround = false;
		}

		if (onGround && Input.GetKeyDown (KeyCode.Z)) {
			transform.rigidbody2D.AddForce (new Vector2 (0, jumpForce));		
		}
	}

	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.tag == "enemy") {
			collideEnemyTime += Time.deltaTime;		

			if(collideEnemyTime > dieTime)
				Hit(col.gameObject.transform.position.x);
		}
	}

	void Hit(float x){
		if(isAlive && state != LifeState.invul)
			Die(x);
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "enemy") {
			collideEnemyTime = 0;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (isAlive && col.gameObject.tag == "enemy") {
		}
		else{
			if(state == LifeState.knockback && col.gameObject.tag == "obstacle"){
				rigidbody2D.velocity = new Vector2(0,0);
				respawnCounter = 0;
				state = LifeState.deadOnGround;
			}
		}
	}

	void Die(float enemyX){
		isAlive = false;
		state = LifeState.knockback;
		atkComboCount = 0;
		float distance = enemyX - transform.position.x;

		rigidbody2D.velocity = new Vector2 (0, 0);

		if (distance > 0) {
			rigidbody2D.AddRelativeForce(new Vector2(-300f, 500f));		
		}
		else{
			rigidbody2D.AddRelativeForce(new Vector2(300f, 500f));		
		}
	}

	public static bool EnemyCanActionPlayer(){
		return isAlive || (state == LifeState.invul);
	}
}	
