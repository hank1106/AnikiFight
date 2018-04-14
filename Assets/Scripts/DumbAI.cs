using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SC;
using DM;

public class DumbAI : MonoBehaviour {

	private Rigidbody2D rg2d;
	
	public int startingHealth = 50;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

	
	private bool leftTurn = false;
	private bool rightTurn = true;
	private bool waiting = false;
	private bool invincible = false;
	private bool trigger = true;
	
	private int attackRate = 20;								//Define the maximum attack speed, the lower the faster.
	private int countCombo = 0;
	private int LightningCoolDown = 0;
	
    private float start_wating_time;
	private float start_invincible_time;
	private float random_decision_time;
	private int playerType = 0;
	private float start_data_record = 0;
	private int accumulated_waiting = 0;

    private const float WAITING_TIME = 0.8f;
	private const float INVINCIBLE_TIME = 1f;
	private const int LIGHT_ATTACK_FREQUENCY = 10;
	private const int DATA_CONVOLUTION_WINDOW = 2;
	private const int HEAVY_ATTACK_FREQUENCY = 20;
	private int ATTACK_WAITING_TIME = 0;
	
    private const int WAIT_FOR_A_WHILE = 0;
	private const int IDLE = -1;
	private const int DIE = 1;
	
	public static bool IS_ANIKI_BEING_ATTACKED = false;
	public Animator anim;
	public static int ThisWin = 0;
	public static int OtherWin = 0;
	
	public static AnimatorClipInfo[] m_CurrentClipInfo;
	BoxCollider2D collider;
	
	private Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains ("Aniki") 
			&& StatusCheck.PlightningStatus 
		    && !StatusCheck.AIlightningStatus) {
			GameControl.instance.Score();
			currentHealth = currentHealth - 5;
			healthSlider.value = currentHealth;
			rg2d.velocity = new Vector2 (0, 10f);
			anim.SetTrigger("hit");
			waiting = true;
            start_wating_time = Time.time;
		}
    }

    public void Combat(int x)
    {
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
		int direct = AIX - playerX > 2f ? -1 : 1;
		if (Random.Range(0, 10f) > 9.5f) {
				anim.SetTrigger("w");
				rg2d.velocity = new Vector2 (10f * direct, 12f);
		} else {
			if (x == 0) {
				if (Random.Range(0, 10f) > 4f)
				{
					anim.SetTrigger("j");
					countCombo++;
					ATTACK_WAITING_TIME = LIGHT_ATTACK_FREQUENCY;
					StatusCheck.AI2getHitType = 1;
					StatusCheck.PgetHitType = 1;

				}
				else
				{
					anim.SetTrigger("k");
					countCombo = 0;
					ATTACK_WAITING_TIME = HEAVY_ATTACK_FREQUENCY;
					StatusCheck.AI2getHitType = 2;
					StatusCheck.PgetHitType = 2;
				}

				IS_ANIKI_BEING_ATTACKED = true;
			} else if (x == 1) {
				if (Random.Range(0, 10f) > 4f)
				{
					
					anim.SetTrigger("k");
					countCombo = 0;
					ATTACK_WAITING_TIME = HEAVY_ATTACK_FREQUENCY;
					StatusCheck.AI2getHitType = 2;
					StatusCheck.PgetHitType = 2;
					IS_ANIKI_BEING_ATTACKED = true;
				}
				
			}
			
			
		}
			
			GameControl.instance.ComboFail();
			
			Model.AIeffectiveAttack ++;
    }

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> ();
		collider = GetComponent<BoxCollider2D>();
		
		// Set the initial health of the player.
        currentHealth = startingHealth;

		rg2d.velocity = new Vector2 (0, 0);
		rg2d.freezeRotation = true;
		healthSlider.value = currentHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
		//GameObject.Find("Blood2").transform.localScale = new Vector3(8.05f * health / 50 , 0.34f, 0);
		float playerX = GameObject.Find("Aniki").transform.position.x;
		float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		StatusCheck.AI2getHitType = 0;
		
		int direct = AIX - playerX > 0 ? -1 : 1;
		
		if (Time.time - start_data_record > DATA_CONVOLUTION_WINDOW) {
			start_data_record = Time.time;
			playerType = Model.DataProcess();
			
			string text = "AIeffectiveAttack: " + Model.AIeffectiveAttack + " " +
						"AIeffectiveDefense: " + Model.AIeffectiveDefense + " " +
						"PeffectiveAttack: " + Model.PeffectiveAttack + " " +
						"PeffectiveDefense: " + Model.PeffectiveDefense + " " +
						"InputAttack: " + Model.InputAttack + " " +
						"RunAway: " + Model.RunAway + "\n ";
		
			using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@"/Users/lihongrui/Desktop/anikiFight/AnikiFight/Assets/Scripts/DataCollector.txt", true))
        	{
				file.WriteLine(text);
        	}
        
		}
        
		IS_ANIKI_BEING_ATTACKED = false;
		StatusCheck.PgetHitType = 0;
		m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
		float YDistance = playerY - AIY;
		float flag = Random.Range(0, 10f);
							
        switch (CheckStatus())
        {
            case WAIT_FOR_A_WHILE:
                if (waiting == true) {
                    if (Time.time - start_wating_time >= WAITING_TIME && accumulated_waiting > 9) {
                        waiting = false;
						invincible = true;
						start_invincible_time = Time.time;
						accumulated_waiting = 0;
                    } else if (Time.time - start_wating_time > WAITING_TIME) {
						waiting = false;
					} else if (accumulated_waiting > 11) {
						waiting = false;
						invincible = true;
						start_invincible_time = Time.time;
						accumulated_waiting = 0;
					}
					
                } else {
                    waiting = true;
                    start_wating_time = Time.time;
                }
				
                break;
				
			case IDLE:
				
				int run = StatusCheck.PositionCheck(playerX, playerY, AIX, AIY, rg2d);
				
				if (m_CurrentClipInfo[0].clip.name == "LightningOn") {
					break;
				}
				if (invincible) {
					if (Time.time - start_invincible_time >= INVINCIBLE_TIME) {
						invincible = false;
					}
				}
				
				if (Input.GetKeyUp(KeyCode.L)) {
					anim.SetTrigger("w");
					rg2d.velocity = new Vector2 (10f * direct, 15f);
				}
				
				if (Time.time - start_wating_time <= WAITING_TIME + 0.2f
				   && Time.time - start_wating_time > WAITING_TIME && Time.time > 5) {
					
					if (playerType < 2 && flag > 5f) {
						anim.SetTrigger("w");
						rg2d.velocity = new Vector2 (-7f * direct, 10f);
						accumulated_waiting = 0;
						
					} else {
						Combat(0);
						accumulated_waiting = 0;
					}
					
				}
				
				if (Time.time - start_wating_time >= WAITING_TIME) {
					 
                        if (run == 0 && attackRate >= ATTACK_WAITING_TIME) {
							Combat(0);
							attackRate = 0;
							accumulated_waiting = 0;
						} else if (run == 1 && attackRate >= ATTACK_WAITING_TIME) {
							Combat(1);
							attackRate = 0;
							accumulated_waiting = 0;
						} else if (run < 2 && playerType == 1) {
							MoveAway();
						} else if (LightningCoolDown >800) {
							
							if (flag > 5f && YDistance < 4f) {
								anim.SetTrigger("lightning");
								StatusCheck.AIlightningPre = true;
								attackRate = 0;
								ATTACK_WAITING_TIME = 10;
								LightningCoolDown = 0;
							} else if (run > 2 && YDistance < 4f) {
								anim.SetTrigger("lightning");
								StatusCheck.AIlightningPre = true;
								attackRate = 0;
								ATTACK_WAITING_TIME = 10;
								LightningCoolDown = 0;
							} else {
								MoveTowards();
							}
							
						} else {
							MoveTowards();
						}
                    }
				
				break;
				
            case DIE:
				anim.SetTrigger("die");
				break;
        }
		
		
		if (m_CurrentClipInfo[0].clip.name == "LightningOn" && trigger) {
			rg2d.velocity = new Vector2 (50 * direct, rg2d.velocity.y);
			StatusCheck.AIlightningPre = false;
			StatusCheck.AIlightningStatus = true;
			attackRate = 0;
			trigger = false;
			
		} else if (m_CurrentClipInfo[0].clip.name == "LightningEnd") {
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			StatusCheck.AIlightningStatus = false;
			collider.enabled = true;
			trigger = true;
		}
		
		attackRate ++;
		LightningCoolDown ++;

	}

    int CheckStatus() {
		int hitResult = StatusCheck.AIBeingHitCheck();

        if (m_CurrentClipInfo[0].clip.name == "idle" || m_CurrentClipInfo[0].clip.name == "jump")
        {
			return -1;
        } else if (hitResult != 0 && !invincible && currentHealth > 0)
        {
			Model.PeffectiveAttack ++;
			currentHealth = hitResult == 1 ? currentHealth - 1 : currentHealth - 3;
			
			// Set the health bar's value to the current health.
        	healthSlider.value = currentHealth;
			
			anim.SetTrigger("hit");
			if (hitResult == 2) {
				float playerX = GameObject.Find("Aniki").transform.position.x;
        		float AIX = GameObject.Find("Enemy").transform.position.x;
				
				if (playerX - AIX > 0) {
					rg2d.velocity = new Vector2(-6f, 0);
				} else {
					rg2d.velocity = new Vector2(6f, 0);
				}
		
			}
			accumulated_waiting ++;
			start_wating_time = Time.time;
			return 0;
        } else if (currentHealth <= 0) {
			OtherWin ++;
			GameControl.instance.AIDead();
			string text = "This AI wins: " + ThisWin + " times " +
						"The Enemy(AI2/Player) wins: " + OtherWin + " times ";
			print(text);
			using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@"/Users/lihongrui/Desktop/anikiFight/AnikiFight/Assets/Scripts/Performance.txt", true))
        	{
				file.WriteLine(text);
        	}
			//SceneManager.LoadScene ("AIvsAI");
			return 1;
		} else {
			return 2;
		}
        
        
    }
	
	void MoveTowards() {
		
		float randnum = Random.Range(0, 10f);
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		
		if (randnum > 9.9f ) {
			randnum = -1;
		} else {
			randnum = 1;
		}
		
		
		
        if(waiting == true)
        {
            if (Time.time - start_wating_time >= WAITING_TIME)
            {
                waiting = false;
            }
        }


        if (waiting == false) {
			
			if (playerY - AIY > 3f) {
				if (Random.Range(0, 10f) > 9.5f) {
					anim.SetTrigger("w");
					rg2d.velocity = new Vector2 (rg2d.velocity.x, 8f);
				}
			}
			
            if (AIX - playerX > 2f)
            {
                if (rg2d.velocity.x < 3f)
                {
                    rg2d.velocity = new Vector2(-3f * randnum, rg2d.velocity.y);
                    if (leftTurn)
                    {
                        rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
                        leftTurn = false;
                        rightTurn = true;
                    }

                }
				else {
					rg2d.velocity = new Vector2(-3f * randnum, rg2d.velocity.y);
				}

            }

            if (AIX - playerX < -2f)
            {
                if (rg2d.velocity.x > -3f)
                {
                    rg2d.velocity = new Vector2(3f * randnum, rg2d.velocity.y);
                    if (rightTurn)
                    {
                        rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
                        rightTurn = false;
                        leftTurn = true;
                    }

                }
				
				else {
					rg2d.velocity = new Vector2(3f * randnum, rg2d.velocity.y);
				}
            }

        }
	}
	
	void MoveAway() {
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
		
		int randnum = 1;
		
        if(waiting == true)
        {
            if (Time.time - start_wating_time >= WAITING_TIME)
            {
                waiting = false;
            }
        }


        if (waiting == false) {
			
			if (Random.Range(0, 10f) > 9.8f) {
				anim.SetTrigger("w");
				rg2d.velocity = new Vector2 (rg2d.velocity.x, 8f);
			}
			
            if (AIX - playerX > 0)
            {
                if (rg2d.velocity.x < 3f)
                {
                    rg2d.velocity = new Vector2(2.5f * randnum, rg2d.velocity.y);
                    if (leftTurn)
                    {
                        rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
                        leftTurn = false;
                        rightTurn = true;
                    }

                } else {
					rg2d.velocity = new Vector2(2.5f * randnum, rg2d.velocity.y);
				}

            }

            if (AIX - playerX < 0)
            {
                if (rg2d.velocity.x > -3f)
                {
                    rg2d.velocity = new Vector2(-2.5f * randnum, rg2d.velocity.y);
                    if (rightTurn)
                    {
                        rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
                        rightTurn = false;
                        leftTurn = true;
                    }

                } else {
					rg2d.velocity = new Vector2(-2.5f * randnum, rg2d.velocity.y);
				}
            }

        }
	}
	
}
