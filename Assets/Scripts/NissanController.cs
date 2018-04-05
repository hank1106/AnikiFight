using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
//using SC;
//using DM;
public class NissanController : NetworkBehaviour {

	private Rigidbody2D rg2d;
	public AudioSource[] arrAllAudioSource;
	public Animator anim;
	AnimatorClipInfo[] m_CurrentClipInfo;
	
	private int attackRate = 1;
	private int LightningCoolDown = 0;
	
	private bool dead = false;
	private bool leftTurn = false;
	private bool rightTurn = true;
	private bool waiting = false;
	private bool invincible = false;
	private bool trigger = true;
	
	private float start_wating_time = 0;
	private float start_invincible_time = 0;

	private const float WAITING_TIME = 1.2f;
	private const float INVINCIBLE_TIME = 1f;
	private const int LIGHT_ATTACK_FREQUENCY = 10;
	private const int HEAVY_ATTACK_FREQUENCY = 20;
	private int ATTACK_WAITING_TIME = 0;
	private int health = 30;
	private bool bloodPos = false;

	private bool lightningStatus = false;
	private int attackType = 0;

	//BoxCollider2D collider;
	//private Animator anim ;

	private GameObject[] players;
	private GameObject enemy;

	private const float LIGHT_ATTACK = 2.5f;
	private const float HEAVY_ATTACK = 3f;

	public Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	private float force  = 3.5f;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> (); 
		//StatusCheck.PlightningStatus = false;
		lightningStatus = false;
		//collider = GetComponent<BoxCollider2D>();
		bloodPos = transform.position.x > 0 ? true : false;
		if (transform.position.x > 0 && rg2d.transform.localScale.x > 0) {
			rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
			leftTurn = true;
			rightTurn = false;
		} else if (transform.position.x < 0 && rg2d.transform.localScale.x < 0) {
			rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
			leftTurn = false;
			rightTurn = true;
		}
		rg2d.freezeRotation = true;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
		if (enemy!=null && enemy!=gameObject && collision.gameObject==enemy && enemy.GetComponent<NissanController>().GetLightningStatus() && !lightningStatus && !dead) 
		{
			/* collision.gameObject.name.Contains ("Enemy") && !StatusCheck.PlightningStatus && StatusCheck.AIlightningStatus */ 
			//health = health - 2;
			CmdHealth(-2);
			CmdHealthBar ();
			rg2d.velocity = new Vector2 (0, 4f);
			//GameControl.instance.ComboFail();
			CmdHit();
			//anim.SetTrigger("hit");
			waiting = true;
            start_wating_time = Time.time;
		}
    }
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;

		if (dead)
			return;

		if (players == null || players.Length < 2) {
			players = GameObject.FindGameObjectsWithTag ("Player");
			if (players.Length == 2) {
				enemy = players [0];
				if (enemy == gameObject)
					enemy = players [1];

				enemy.GetComponent<SpriteRenderer> ().material.color = Color.yellow;

				/*
				if (enemy.transform.position.x > 0) {
					if(transform.position.x-enemy.transform.position.x>2f && rg2d.transform.localScale.x>0)
						rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
					//enemy.GetComponent<AnikiOne>().FlipOnStart (true);
				}
				*/
			}
		}
		/*
		float player = GameObject.Find("Aniki").transform.position.x;
		float AI = GameObject.Find("Enemy").transform.position.x;
		*/
		float player = transform.position.x;
		//StatusCheck.AIgetHitType = 0;
		CmdSetAttackType (0);
		//CmdHealthBar ();


		if (health<=0 && dead==false) {
			CmdDie ();
		} else {
			if (!waiting) {
				if (enemy!=null && enemy!=gameObject && !dead) {
					float AI = enemy.transform.position.x;
					//Debug.Log ("Change Position!");
					if (AI - player > 2f) {
						if (leftTurn) {
							//rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
							FlipPlayer (false);
						}
					}

					if (AI - player < -2f) {
						if (rightTurn) {
							//rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
							FlipPlayer (true);
						}
					}
				}

				if (isLocalPlayer && !dead) {
					if (Input.GetKeyDown(KeyCode.D)) {
						if (rg2d.velocity.x < 3f) {
							rg2d.velocity = new Vector2 (force, rg2d.velocity.y);
						}
					}

					if (Input.GetKeyUp(KeyCode.D)) {
						rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
					}

					if (Input.GetKeyDown(KeyCode.A)) {
						if (rg2d.velocity.x > -3f) {
							rg2d.velocity = new Vector2 (-force, rg2d.velocity.y);
						}
					}

					if (Input.GetKeyUp(KeyCode.A)) {
						rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
					}

					if (Input.GetKeyDown(KeyCode.W)) {
						rg2d.velocity = new Vector2 (rg2d.velocity.x, 18f);
						CmdJump();
						//anim.SetTrigger("w");
					}

					if (Input.GetKeyDown(KeyCode.J) && attackRate > ATTACK_WAITING_TIME) {
						//anim.SetTrigger("j");
						CmdPunch();
						//Model.InputAttack ++;
						//StatusCheck.AIgetHitType = 1;
						CmdSetAttackType(1);
						attackRate = 0;
						ATTACK_WAITING_TIME = LIGHT_ATTACK_FREQUENCY;
					}

					if (Input.GetKeyDown(KeyCode.K) && attackRate > ATTACK_WAITING_TIME) {
						//anim.SetTrigger("k");
						CmdKick();
						//StatusCheck.AIgetHitType = 2;
						CmdSetAttackType(2);
						//Model.InputAttack ++;
						attackRate = 0;
						ATTACK_WAITING_TIME = HEAVY_ATTACK_FREQUENCY;
					}

					if (Input.GetKeyDown(KeyCode.L) && LightningCoolDown == 0) {
						//anim.SetTrigger("lightning");
						CmdLightning();
						//Model.InputAttack ++;
						attackRate = 0;
						ATTACK_WAITING_TIME = 10;
						LightningCoolDown = 150;

					}
				}
				
				if (invincible) {
					if(Time.time - start_invincible_time >= INVINCIBLE_TIME) {
                    	invincible = false;
                	}
				}
			
			} else {
			 if (Time.time - start_wating_time >= WAITING_TIME) {
                    waiting = false;
				 	start_invincible_time = Time.time;
				 	invincible = true;
                }
			}
		
			if (isBeingHit () && !dead) {
				rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
				if (!waiting) {
					waiting = true;
					start_wating_time = Time.time;	
				}
			}
			
		}
		
		m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
		
		if (m_CurrentClipInfo[0].clip.name == "LightningOn" && trigger && !dead) {
			
			//float playerX = GameObject.Find("Aniki").transform.position.x;
        	//float AIX = GameObject.Find("Enemy").transform.position.x;
			invincible = true;
			if (enemy!=null && enemy!=gameObject && enemy.GetComponent<NissanController>().ifAlive()) {
				float playerX = gameObject.transform.position.x;
				float AIX = enemy.transform.position.x;
				int direct = AIX - playerX > 0 ? 1 : -1;
				rg2d.velocity = new Vector2 (50 * direct, rg2d.velocity.y);
			} else {
				rg2d.velocity = new Vector2 (50, rg2d.velocity.y);
			}

			//StatusCheck.PlightningStatus = true;
			//lightningStatus = true;
			CmdSetLightningStatus(true);
			trigger = false;
			
		} else if (m_CurrentClipInfo[0].clip.name == "LightningEnd" && !dead) {
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			//StatusCheck.PlightningStatus = false;
			//lightningStatus = false;
			CmdSetLightningStatus(false);
			GetComponent<Collider>().enabled = true;
			trigger = true;
		}
		
		attackRate ++;
		LightningCoolDown = LightningCoolDown > 0 ? LightningCoolDown - 1 : 0;
    }

	int BeingHitCheck(){
		if (enemy == null || enemy == gameObject)
			return 0;
		float playerX = gameObject.transform.position.x;
		float playerY = gameObject.transform.position.y;
		float AIX = enemy.transform.position.x;
		float AIY = enemy.transform.position.y;
		float distance = Mathf.Sqrt((AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY));
		//Debug.Log ("Distance: "+distance.ToString());
		if (enemy.GetComponent<NissanController>().GetAttackType() == 1 && distance <= LIGHT_ATTACK)
		{
			return 1;
		} else if (enemy.GetComponent<NissanController>().GetAttackType() == 2 && distance <= HEAVY_ATTACK) {
			return 2;
		}
		return 0;
	}
	
	bool isBeingHit() 
    {
		if(enemy!=null && enemy!=gameObject && BeingHitCheck()!=0 && !dead && !invincible)
        {
			// DumbAI.IS_ANIKI_BEING_ATTACKED && !dead && !invincible
			//anim.SetTrigger("hit");
			CmdHit();
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			//health--;
			CmdHealth(-1);
			CmdHealthBar ();
        	return true;
        }
        else
        {
        	return false;
        }
    }

	public bool ifAlive(){
		return !dead;
	}

	public override void OnStartLocalPlayer(){
		//GetComponent<SpriteRenderer> ().material.color = Color.yellow;
	}

	bool GetLightningStatus (){
		return lightningStatus;
	}

	int GetAttackType(){
		return attackType;
	}

	void FlipOnStart(bool var){
		FlipPlayer (var);
	}

	void FlipPlayer(bool var){
		CmdFlipPlayer ();
		leftTurn = var;
		rightTurn = !var;
	}

	[ClientRpc]
	void RpcHealthBar(){
		if(bloodPos) GameObject.Find("Blood2").transform.localScale = new Vector3(8.05f * health / 30 , 0.34f, 0);
		else GameObject.Find("Blood1").transform.localScale = new Vector3(8.05f * health / 30 , 0.34f, 0);
	}

	[ClientRpc]
	void RpcFlipPlayer(){
		rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
	}

	[ClientRpc]
	void RpcHealth(int var){
		health += var;
	}
    
	[ClientRpc]
	void RpcDie(){
		anim.SetTrigger ("die");
		dead = true;
		//Destroy (rg2d);
		//collider.enabled = false;
	}

	[ClientRpc]
	void RpcSetAttackType(int val){
		attackType = val;
	}

	[ClientRpc]
	void RpcSetLightningStatus(bool val){
		lightningStatus = val;
	}

	[ClientRpc]
	void RpcJump(){
		anim.SetTrigger ("w");
	}

	[ClientRpc]
	void RpcPunch(){
		anim.SetTrigger ("j");
	}

	[ClientRpc]
	void RpcKick(){
		anim.SetTrigger ("k");
	}

	[ClientRpc]
	void RpcLightning(){
		anim.SetTrigger("lightning");
	}

	[ClientRpc]
	void RpcHit(){
		anim.SetTrigger ("hit");
	}

	[Command]
	void CmdHealthBar(){
		//anim.SetTrigger ("w");
		RpcHealthBar ();
	}

	[Command]
	void CmdFlipPlayer(){
		RpcFlipPlayer ();
	}

	[Command]
	void CmdHealth(int var){
		//anim.SetTrigger ("w");
		RpcHealth (var);
	}

	[Command]
	void CmdDie(){
		RpcDie ();
	}

	[Command]
	void CmdSetAttackType(int var){
		RpcSetAttackType (var);
	}

	[Command]
	void CmdSetLightningStatus(bool var){
		RpcSetLightningStatus (var);
	}

	[Command]
	void CmdJump(){
		//anim.SetTrigger ("w");
		RpcJump ();
	}

	[Command]
	void CmdPunch(){
		//anim.SetTrigger ("j");
		RpcPunch ();
	}

	[Command]
	void CmdKick(){
		//anim.SetTrigger ("k");
		RpcKick ();
	}

	[Command]
	void CmdLightning(){
		RpcLightning ();
	}

	[Command]
	void CmdHit(){
		RpcHit ();
	}
}
	

