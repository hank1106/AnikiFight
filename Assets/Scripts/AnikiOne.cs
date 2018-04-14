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
	public static AnimatorClipInfo[] m_CurrentClipInfo;
	
	public int startingHealth = 50;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
   
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

	
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
	BoxCollider2D collider;
	//private Animator anim ;

	public Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	private float force  = 3.5f;
	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		anim = GetComponent<Animator>();
		arrAllAudioSource = GetComponents<AudioSource>();
		rg2d = GetComponent<Rigidbody2D> (); 
		StatusCheck.PlightningStatus = false;
		collider = GetComponent<BoxCollider2D>();
		rg2d.freezeRotation = true;
		healthSlider.value = currentHealth;
		
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains ("Enemy") 
			&& !StatusCheck.PlightningStatus 
		    && StatusCheck.AIlightningStatus) {
			
			currentHealth = currentHealth - 5;
			healthSlider.value = currentHealth;
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
		m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
		
		if (currentHealth <= 0 && !dead) {
			anim.SetTrigger("die");
			GameControl.instance.PlayerDead();
			arrAllAudioSource[1].Play();
			dead = true;
		} else {
			
			if (!waiting && (m_CurrentClipInfo[0].clip.name == "idle" || m_CurrentClipInfo[0].clip.name == "jump")) {
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
				
				if (Input.GetKeyDown(KeyCode.I)) {
					rg2d.velocity = new Vector2 (0, 0);
					arrAllAudioSource[3].Play();
					//anim.SetTrigger("i");
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
					arrAllAudioSource[2].Play();
					StatusCheck.AIgetHitType = 2;
					Model.InputAttack ++;
					attackRate = 0;
				}
				
				if (Input.GetKeyDown(KeyCode.L) && LightningCoolDown == 0) {
					arrAllAudioSource[0].Play();
					anim.SetTrigger("lightning");
					
					Model.InputAttack ++;
					StatusCheck.PlightningPre = true;
					background.anim.SetTrigger("pre");
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
			 	} else if (accumulated_waiting > 11) {
						waiting = false;
						invincible = true;
						start_invincible_time = Time.time;
						accumulated_waiting = 0;
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
		
		if (m_CurrentClipInfo[0].clip.name == "LightningOn" && trigger) {
			StatusCheck.PlightningPre = false;
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
        if(StatusCheck.PBeingHitCheck() > 0 && currentHealth > 0 && !invincible)
        {
			int damage = 0;
			anim.SetTrigger("hit");
			start_wating_time = Time.time;	
			accumulated_waiting ++;
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			
			if (StatusCheck.PBeingHitCheck() == 1) {
				damage = 1;
			} else if (StatusCheck.PBeingHitCheck() == 2) {
				damage = 5;
				float playerX = GameObject.Find("Aniki").transform.position.x;
        		float AIX = GameObject.Find("Enemy").transform.position.x;
				
				if (playerX - AIX > 0) {
					rg2d.velocity = new Vector2(6f, 0);
				} else {
					rg2d.velocity = new Vector2(-6f, 0);
				}
			}
			
			currentHealth -= damage;
			healthSlider.value = currentHealth;

        	return true;
        }
        else
        {
        	return false;
        }
    }
           

}
	

