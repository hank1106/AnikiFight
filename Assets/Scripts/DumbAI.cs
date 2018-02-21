using System.Collections;
using System.Collections.Generic;

using System;
using System.Globalization;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SC;



namespace DA {
	public class DumbAI : MonoBehaviour {

		public static bool IS_ANIKI_BEING_ATTACKED = false;
		private Rigidbody2D rg2d;
		private bool leftTurn = false;
		private bool rightTurn = true;
		private int countCombo = 0;
    	private bool waiting = false;
    	private float start_wating_time;

    	private const float WAITING_TIME = 3.0f;
    	private const int WAIT_FOR_A_WHILE = 0;

    

    	//Random random = new Random();
	
	
		public AudioSource[] arrAllAudioSource;
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
        	}
            if(StatusCheck.WithinAttackDistance())
            {
                IS_ANIKI_BEING_ATTACKED = true;
            }
            else
            {
                IS_ANIKI_BEING_ATTACKED = false;
            }
    	}

    	// Use this for initialization
    	void Start () {
			anim = GetComponent<Animator>();
			rg2d = GetComponent<Rigidbody2D> (); 

			//rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
		
			if (SceneManager.GetActiveScene ().name == "Level0")

			rg2d.velocity = new Vector2 (0, 0);
			arrAllAudioSource = GetComponents<AudioSource>();

		}
	
		// Update is called once per frame
		void Update () {
			Combat();
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
                    	rg2d.velocity = new Vector2(-3f * UnityEngine.Random.Range(0.1f, 0.7f), rg2d.velocity.y);
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
                    	rg2d.velocity = new Vector2(3f * UnityEngine.Random.Range(0.1f, 0.7f), rg2d.velocity.y);
                    	if (rightTurn)
                    	{
                        	rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
                        	rightTurn = false;
                        	leftTurn = true;
                    	}

                	}
            	}

        	}
        
        

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
                

                	//rg2d.velocity = new Vector2(0, rg2d.velocity.y);
                	break;
            	default:
                	break;
        	}
        //StatusCheck();
		
		//float distance = (AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY);

//		if (AIX - playerX > 2f) {
//			if (rg2d.velocity.x < 3f) {
//				rg2d.velocity = new Vector2 (-3f, rg2d.velocity.y);
//				if (leftTurn) {
//					 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
//					leftTurn = false;
//					rightTurn = true;
//				}
//				
//			}
//		}
//		if (distance < 4f) {
//			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
//			if (countCombo < 2) {
//				anim.SetTrigger("j");
//				countCombo ++;
//			}else{
//				anim.SetTrigger("k");
//			}
//			
//		}
//
//       if (AIX - playerX < -2f) {
//		   if (rg2d.velocity.x > -3f) {
//				rg2d.velocity = new Vector2 (3f, rg2d.velocity.y);
//			   if (rightTurn) {
//					 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
//					rightTurn = false;
//					leftTurn = true;
//				}
//			   
//			}
//	   }
		
	
		
	
           

		}

    	int CheckStatus() 
        {
        	float playerX = GameObject.Find("Aniki").transform.position.x;
        	float AIX = GameObject.Find("Enemy").transform.position.x;
        	float playerY = GameObject.Find("Aniki").transform.position.y;
        	float AIY = GameObject.Find("Enemy").transform.position.y;

        	if (StatusCheck.BeingHitCheck())
        	{
            	return 0;
        	}
        	else
        	{

        	}
        	return -1;
    	}
	
	}
}
