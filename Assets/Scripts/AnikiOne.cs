using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SC;
using DM;
public class AnikiOne : MonoBehaviour {

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
	private int accumulated_waiting = 0;
	
	private const float WAITING_TIME = 0.4f;
	private const float INVINCIBLE_TIME = 1f;
	private const int LIGHT_ATTACK_FREQUENCY = 10;
	private const int HEAVY_ATTACK_FREQUENCY = 20;
	private int ATTACK_WAITING_TIME = 0;
	private int health = 30;
	BoxCollider2D collider;
	//private Animator anim ;

	public Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	private float force  = 3.5f;
	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> (); 
		StatusCheck.PlightningStatus = false;
		collider = GetComponent<BoxCollider2D>();
		rg2d.freezeRotation = true;
		
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains ("Enemy") 
			&& !StatusCheck.PlightningStatus 
		    && StatusCheck.AIlightningStatus) {
			
			health = health - 2;
			rg2d.velocity = new Vector2 (0, 4f);
			GameControl.instance.ComboFail();
			anim.SetTrigger("hit");
			waiting = true;
            start_wating_time = Time.time;
		}
    }
	
	// Update is called once per frame
	void Update () {
		float player = GameObject.Find("Aniki").transform.position.x;
		float AI = GameObject.Find("Enemy").transform.position.x;
		StatusCheck.AIgetHitType = 0;
		
		GameObject.Find("Blood1").transform.localScale = new Vector3(8.05f * health / 30 , 0.34f, 0);
		
		if (health <= 0) {
			anim.SetTrigger("die");
			dead = true;
		} else {
			
			if (!waiting) {
				if (AI - player > 2f) {
					if (leftTurn) {
						 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
						leftTurn = false;
						rightTurn = true;
					}
				}

				if (AI - player < -2f) {
					   if (rightTurn) {
							 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
							rightTurn = false;
							leftTurn = true;
						}
			   }

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
					anim.SetTrigger("w");
				}

				if (Input.GetKeyDown(KeyCode.J) && attackRate > ATTACK_WAITING_TIME) {
					anim.SetTrigger("j");
					Model.InputAttack ++;
					StatusCheck.AIgetHitType = 1;
					attackRate = 0;
					ATTACK_WAITING_TIME = LIGHT_ATTACK_FREQUENCY;
				}

				if (Input.GetKeyDown(KeyCode.K) && attackRate > ATTACK_WAITING_TIME) {
					anim.SetTrigger("k");
					StatusCheck.AIgetHitType = 2;
					Model.InputAttack ++;
					attackRate = 0;
					ATTACK_WAITING_TIME = HEAVY_ATTACK_FREQUENCY;
				}
				
				if (Input.GetKeyDown(KeyCode.L) && LightningCoolDown == 0) {
					anim.SetTrigger("lightning");
					Model.InputAttack ++;
					attackRate = 0;
					ATTACK_WAITING_TIME = 10;
					LightningCoolDown = 150;
					
				}
				
				
				if (invincible) {
					if(Time.time - start_invincible_time >= INVINCIBLE_TIME) {
                    	invincible = false;
                	}
				}
				
				
			} else {
			 if (Time.time - start_wating_time >= WAITING_TIME && accumulated_waiting > 9) {
                    waiting = false;
				 	start_invincible_time = Time.time;
				 	invincible = true;
				 	accumulated_waiting = 0;
                } else if (Time.time - start_wating_time >= WAITING_TIME) {
				  	waiting = false;
			 	}
				
			}
		
		
		
		if (isBeingHit()) {
				rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			
                if (!waiting) {
					waiting = true;
					start_wating_time = Time.time;	
				}
        	}
			
		}
		
		m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
		
		if (m_CurrentClipInfo[0].clip.name == "LightningOn" && trigger) {
			
			float playerX = GameObject.Find("Aniki").transform.position.x;
        	float AIX = GameObject.Find("Enemy").transform.position.x;
			int direct = AIX - playerX > 2f ? 1 : -1;
			invincible = true;
			rg2d.velocity = new Vector2 (50 * direct, rg2d.velocity.y);
			StatusCheck.PlightningStatus = true;
			trigger = false;
			
		} else if (m_CurrentClipInfo[0].clip.name == "LightningEnd") {
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			StatusCheck.PlightningStatus = false;
			collider.enabled = true;
			trigger = true;
		}
		
		attackRate ++;
		LightningCoolDown = LightningCoolDown > 0 ? LightningCoolDown - 1 : 0;
    }
	
	bool isBeingHit() 
    {
        if(DumbAI.IS_ANIKI_BEING_ATTACKED && !dead && !invincible)
        {
			anim.SetTrigger("hit");
			start_wating_time = Time.time;	
			accumulated_waiting ++;
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			health--;
        	return true;
        }
        else
        {
        	return false;
        }
    }
           

}
	

