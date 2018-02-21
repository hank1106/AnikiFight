using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SC;
using DA;
public class AnikiOne : MonoBehaviour {

	private Rigidbody2D rg2d;
	public AudioSource[] arrAllAudioSource;
	public Animator anim;
	private bool leftTurn = false;
	private bool rightTurn = true;
	private bool waiting = false;
	private float start_wating_time;
	//private Animator anim ;

	public Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	public void SetForce(float ttt) {
		this.force = ttt;
	}

	private float force  = 3.5f;
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
		if(waiting == false)
		{
			float player = GameObject.Find("Aniki").transform.position.x;
			float AI = GameObject.Find("Enemy").transform.position.x;
		
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
		
			if (Input.GetKeyDown(KeyCode.J)) {
				anim.SetTrigger("j");
			}
		
			if (Input.GetKeyDown(KeyCode.K)) {
				anim.SetTrigger("k");
			}
		}
		if(isBeingHit())
        {
           	if(waiting == true)
            {
                print(Time.time - start_wating_time);
                if(Time.time - start_wating_time >= 0.5)
                {
                    waiting = false;
                }
            }
            else
            {
                waiting = true;
                start_wating_time = Time.time;
            }
        }
    }

    bool isBeingHit() 
    {
        if(DumbAI.IS_ANIKI_BEING_ATTACKED)
        {
        	return true;
        }
        else
        {
        	return false;
        }
    }

}
