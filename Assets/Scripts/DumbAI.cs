using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DumbAI : MonoBehaviour {

	private Rigidbody2D rg2d;
	private bool leftTurn = false;
	private bool rightTurn = true;
	private int countCombo = 0;
	
	
	public AudioSource[] arrAllAudioSource;
	public Animator anim;
	private float force  = 0;

	private Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	private void SetForce(float ttt) {
		this.force = ttt;
	}
	
	private void PositionCheck(float playerX, float playerY, float AIX, float AIY) {
		float distance = (AIX - playerX) * (AIX - playerX) + (AIY - playerY) * (AIY - playerY);
		
		if (AIX - playerX > 2f) {
			if (rg2d.velocity.x < 3f) {
				rg2d.velocity = new Vector2 (-3f, rg2d.velocity.y);
				if (leftTurn) {
					 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
					leftTurn = false;
					rightTurn = true;
				}
				
			}
		}
		if (distance < 4f) {
			rg2d.velocity = new Vector2 (0, rg2d.velocity.y);
			
			Combat();
		}

       if (AIX - playerX < -2f) {
		   if (rg2d.velocity.x > -3f) {
				rg2d.velocity = new Vector2 (3f, rg2d.velocity.y);
			   if (rightTurn) {
					 rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
					rightTurn = false;
					leftTurn = true;
				}
			   
			}
	   }
	}
	
	private void StatusCheck() {
		
		float playerX = GameObject.Find("Aniki").transform.position.x;
		float AIX = GameObject.Find("Enemy").transform.position.x;
		float playerY = GameObject.Find("Aniki").transform.position.y;
		float AIY = GameObject.Find("Enemy").transform.position.y;
		
		if (!BeingHitCheck()) {
			
			PositionCheck(playerX, playerY, AIX, AIY);
			
		}
	}
	
	private bool BeingHitCheck() {
		return false;
	}
	
	private void Combat() {
		if (countCombo < 2) {
				anim.SetTrigger("j");
				countCombo ++;
			}else{
				anim.SetTrigger("k");
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
//		float playerX = GameObject.Find("Aniki").transform.position.x;
//		float AIX = GameObject.Find("Enemy").transform.position.x;
//		float playerY = GameObject.Find("Aniki").transform.position.y;
//		float AIY = GameObject.Find("Enemy").transform.position.y;
		
		StatusCheck();
		
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
	
}
