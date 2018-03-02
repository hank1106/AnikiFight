using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SC;
using DM;

public class DumbAI : MonoBehaviour {

	private Rigidbody2D rg2d;
	private bool leftTurn = false;
	private bool rightTurn = true;
	private bool waiting = false;
	private bool invincible = false;
	
	private int attackRate = 20;
	private int countCombo = 0;
	private int LightningCoolDown = 0;
	
    private float start_wating_time;
	private float start_invincible_time;
	private float random_decision_time;
	private int playerType = -1;
	private int health = 25;

    private const float WAITING_TIME = 2f;
	private const float INVINCIBLE_TIME = 0.7f;
	private const int LIGHT_ATTACK_FREQUENCY = 10;
	private const int HEAVY_ATTACK_FREQUENCY = 20;
	private int ATTACK_WAITING_TIME = 0;
	
    private const int WAIT_FOR_A_WHILE = 0;
	private const int IDLE = -1;
	private const int DIE = 1;
	
	public static bool IS_ANIKI_BEING_ATTACKED = false;
	public Animator anim;
	AnimatorClipInfo[] m_CurrentClipInfo;
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
			health = health - 2;
			rg2d.velocity = new Vector2 (0, 10f);
			anim.SetTrigger("hit");
			waiting = true;
            start_wating_time = Time.time;
		}
    }

    public void Combat()
    {
        if (countCombo < 2)
        {
            anim.SetTrigger("j");
            countCombo++;
			ATTACK_WAITING_TIME = LIGHT_ATTACK_FREQUENCY;
        }
        else
        {
            anim.SetTrigger("k");
			countCombo = 0;
			ATTACK_WAITING_TIME = HEAVY_ATTACK_FREQUENCY;
        }
		GameControl.instance.ComboFail();
		IS_ANIKI_BEING_ATTACKED = true;
		Model.AIeffectiveAttack ++;
		
    }

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> ();
		collider = GetComponent<BoxCollider2D>();

		if (SceneManager.GetActiveScene ().name == "Level0")

		rg2d.velocity = new Vector2 (0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject.Find("Blood2").transform.localScale = new Vector3(8.05f * health / 25 , 0.34f, 0);
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		
		int direct = AIX - playerX > 2f ? -1 : 1;
		
		if (Time.time > 15) {
			playerType = Model.DataProcess();
		}
        
		IS_ANIKI_BEING_ATTACKED = false;

        switch (CheckStatus())
        {
            case WAIT_FOR_A_WHILE:
                if (waiting == true) {
					
                    if (Time.time - start_wating_time >= WAITING_TIME) {
                        waiting = false;
						invincible = true;
						start_invincible_time = Time.time;
						rg2d.velocity = new Vector2 (-5f * direct, 10f);
                    }
					
                } else {
                    waiting = true;
                    start_wating_time = Time.time;
                }
                
                break;
				
			case IDLE:
				if (invincible) {
					if (start_invincible_time - Time.time >= INVINCIBLE_TIME) {
						invincible = false;
					}
				}
				
				if (Time.time - start_wating_time >= WAITING_TIME) {
					 
						int run = StatusCheck.PositionCheck(playerX, playerY, AIX, AIY, rg2d);
					
                        if (run == 1 && attackRate >= ATTACK_WAITING_TIME) {
							Combat();
							attackRate = 0;
						} else if (run == 0 && playerType == 1) {
							MoveAway();
						} else if (run ==2 && LightningCoolDown >1000) {
							anim.SetTrigger("lightning");
							attackRate = 0;
							ATTACK_WAITING_TIME = 10;
							LightningCoolDown = 0;
						} else {
							MoveTowards();
						}
                    }
				
				break;
				
            case DIE:
				anim.SetTrigger("die");
				break;
        }
		
		m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
		
		if (m_CurrentClipInfo[0].clip.name == "LightningOn") {
		
			rg2d.velocity = new Vector2 (50 * direct, rg2d.velocity.y);
			StatusCheck.AIlightningStatus = true;
			attackRate = 0;
			
		} else if (m_CurrentClipInfo[0].clip.name == "LightningEnd") {
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			StatusCheck.AIlightningStatus = false;
			collider.enabled = true;
		}
		
		attackRate ++;
		LightningCoolDown ++;

	}

    int CheckStatus() {
		int hitResult = StatusCheck.AIBeingHitCheck();

        if (hitResult != 0 && !invincible)
        {
			Model.PeffectiveAttack ++;
			health = hitResult == 1 ? health - 1 : health - 2;
			anim.SetTrigger("hit");
			return 0;
        } else if (health <= 0) {
			return 1;
		}
        else
        {
			return -1;
        }
        
    }
	
	void MoveTowards() {
		
		float randnum = Random.Range(0, 10f);
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		
		if (randnum > 9.7f ) {
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
//			if (Random.Range(0, 10f) > 9.9f) {
//				anim.SetTrigger("w");
//				rg2d.velocity = new Vector2 (rg2d.velocity.x, 3f);
//			}
			
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
            }

        }
	}
	
	void MoveAway() {
		float randnum = Random.Range(0, 10f);
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
		
		if (randnum > 9.7f ) {
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
			
			if (Random.Range(0, 10f) > 9.8f) {
				anim.SetTrigger("w");
				rg2d.velocity = new Vector2 (rg2d.velocity.x, 8f);
			}
			
            if (AIX - playerX > 2f)
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

                }

            }

            if (AIX - playerX < -2f)
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

                }
            }

        }
	}
	
}
