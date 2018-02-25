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
	private int countCombo = 0;
    private bool waiting = false;
    private float start_wating_time;
	private float random_decision_time;
	private int playerType = -1;
	private int health = 10;

    private const float WAITING_TIME = 3.0f;
	
    private const int WAIT_FOR_A_WHILE = 0;
	private const int IDLE = -1;
	private const int DIE = 1;
	
	public static bool IS_ANIKI_BEING_ATTACKED = false;
	public Animator anim;
	private float force  = 0;

	private Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	private void SetForce(float ttt) {
		this.force = ttt;
	}

    public void Combat()
    {
        if (countCombo < 2)
        {
            anim.SetTrigger("j");
            countCombo++;
        }
        else
        {
            anim.SetTrigger("k");
			countCombo = 0;
        }
		
		IS_ANIKI_BEING_ATTACKED = true;
		
    }

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> (); 

			//rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
		
		if (SceneManager.GetActiveScene ().name == "Level0")

		rg2d.velocity = new Vector2 (0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		
		if (Time.time > 15) {
			playerType = Model.DataProcess();
		}
        
		IS_ANIKI_BEING_ATTACKED = false;

        switch(CheckStatus())
        {
            case WAIT_FOR_A_WHILE:
                if(waiting == true)
                {
                    if(Time.time - start_wating_time >= 2)
                    {
                        waiting = false;
                    }
                }
                else
                {
                    waiting = true;
                    start_wating_time = Time.time;
                }
                
                break;
				
			case IDLE:
				
				if(Time.time - start_wating_time >= 2)
                    {
						int run = StatusCheck.PositionCheck(playerX, playerY, AIX, AIY, rg2d);
                        if (run == 1) {
							Combat();
						} else if (run == 0 && playerType == 1) {
							MoveAway();
						} else {
							MoveTowards();
						}
                    }
				
				break;
				
            case DIE:
				anim.SetTrigger("die");
                break;
        }
		
		

	}

    int CheckStatus() {
        float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;

        if (StatusCheck.BeingHitCheck())
        {
			Model.PeffectiveAttack ++;
			health--;
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
		
		if (randnum > 9.5f ) {
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
			if (Random.Range(0, 10f) > 9.5f) {
				anim.SetTrigger("w");
				rg2d.velocity = new Vector2 (rg2d.velocity.x, 8f);
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
		float playerX = GameObject.Find("Aniki").transform.position.x;
        float AIX = GameObject.Find("Enemy").transform.position.x;
        float playerY = GameObject.Find("Aniki").transform.position.y;
        float AIY = GameObject.Find("Enemy").transform.position.y;
		
        if(waiting == true)
        {
            if (Time.time - start_wating_time >= WAITING_TIME)
            {
                waiting = false;
            }
        }


        if (waiting == false) {
            if (AIX - playerX > 2f)
            {
                if (rg2d.velocity.x < 3f)
                {
                    rg2d.velocity = new Vector2(2.5f, rg2d.velocity.y);
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
                    rg2d.velocity = new Vector2(-2.5f, rg2d.velocity.y);
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
