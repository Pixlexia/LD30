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
	public const float defaultGravity = 4f;
	public const float waterGravity = 0.1f;
	public const float flyGravity = 1f;
	bool onGround;
	public bool hasWings, canFly, isFlying;
	public bool hasSwim, isSwimming;
	const float maxFlySpeed = 6;
	const float maxSwimSpeed = -2;

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

		respawnTime = 1.5f; // time before player respawns from deadonground
		respawnCounter = 0;

		invulTime = 1.5f; // time player is invulnerable upon respawning
		invulCounter = 0;

		// physics
		canFly = false;
		isFlying = false;
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

		Physics2D.IgnoreLayerCollision(10, 11, (this.rigidbody2D.velocity.y > 0.0f)); 

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

		Play.Shake (0.05f);

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
//			rigidbody2D.AddRelativeForce(new Vector2(-speed,0));
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
		if(hasWings) 
			FlyControls ();
		if(hasSwim)
			SwimControls ();
	}

	void FlyControls(){
		if (!onGround) {
			if(Input.GetKeyUp (KeyCode.Z)){
				canFly = true;
			}
		}

		if(canFly && Input.GetKey (KeyCode.Z)) {
			isFlying = true;
			rigidbody2D.gravityScale = flyGravity;
			if(rigidbody2D.velocity.y < maxFlySpeed)
				transform.rigidbody2D.AddForce(new Vector2(0, 30));		
		}
	}

	void SwimControls(){
		if (isSwimming) {
			if(rigidbody2D.velocity.y < maxSwimSpeed){
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSwimSpeed);
			}
		} 
		else {
		}
	}

	void Death(){
		switch (state) {
		case LifeState.deadOnGround:
			respawnCounter += Time.deltaTime;
			if(respawnCounter > respawnTime){
				// invul period
				TurnOnCollisionWithEnemies();
				state = LifeState.invul;
				invulCounter = 0;
				GetComponent<BlinkingSprite>().blinking = true;
			}
			break;

		case LifeState.invul:
			invulCounter += Time.deltaTime;
			if(invulCounter > invulTime){
				// respawn player
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
		}
		else{
			onGround = false;
		}

		if (onGround && Input.GetKeyDown (KeyCode.Z)) {
			transform.rigidbody2D.AddForce (new Vector2 (0, jumpForce));		
		}
	}

	void StartSwim(){
		if (state == LifeState.knockback) {
			state = LifeState.deadOnGround;	
		}
		rigidbody2D.gravityScale = waterGravity;
		isSwimming = true;
		canFly = false;
	}

	void EndSwim(){
		rigidbody2D.gravityScale = defaultGravity;
		isSwimming = false;
		canFly = true;
	}

	void TouchGround(){
		onGround = true;
		canFly = false;
		EndFly ();
	}

	void EndFly(){
		isFlying = false;
		rigidbody2D.gravityScale = defaultGravity;
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

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "water") {
			if(hasSwim || state == LifeState.knockback)
				StartSwim();
			else
				Die (-1);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "water"){
			EndSwim();
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "enemy") {
			collideEnemyTime = 0;
		}

		if (col.gameObject.tag == "water")
			isSwimming = false;
	}

	void OnCollisionEnter2D(Collision2D col){
		if (isAlive && col.gameObject.tag == "enemy") {
		}
		else if(col.gameObject.tag == "enemybullet"){
			Hit (col.gameObject.transform.position.x);

			if(col.gameObject.GetComponent<Bullet>() != null)
				col.gameObject.GetComponent<Bullet>().Die();
			else if(col.gameObject.GetComponent<Tentacle>() != null)
				col.gameObject.GetComponent<Tentacle>().Die();
		}
		else{
			if(col.gameObject.tag == "obstacle" && rigidbody2D.velocity.y == 0)
				TouchGround();

			if(state == LifeState.knockback && col.gameObject.tag == "obstacle"){
				rigidbody2D.velocity = new Vector2(0,0);
				respawnCounter = 0;
				state = LifeState.deadOnGround;
			}
		}
	}

	void TurnOffCollisionWithEnemies(){
		Physics2D.IgnoreLayerCollision (10, 8);
		Physics2D.IgnoreLayerCollision (10, 9);
	}

	void TurnOnCollisionWithEnemies(){
		Physics2D.IgnoreLayerCollision(10, 8, false);
		Physics2D.IgnoreLayerCollision(10, 9, false);
	}

	void Die(float enemyX){
		TurnOffCollisionWithEnemies ();

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
