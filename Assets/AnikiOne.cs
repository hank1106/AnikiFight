using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class AnikiOne : MonoBehaviour {

	private Rigidbody2D rg2d;
	public AudioSource[] arrAllAudioSource;
	public Animator anim;
	//private Animator anim ;

	public Rigidbody2D GetRigidbody2D() {
		return this.rg2d;
	}

	public void SetForce(float ttt) {
		this.force = ttt;
	}

	private float force  = 3f;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rg2d = GetComponent<Rigidbody2D> (); 

			rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
		
		if (SceneManager.GetActiveScene ().name == "Level0")
			force = 3.5f;

		rg2d.velocity = new Vector2 (force, 0);
		arrAllAudioSource = GetComponents<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
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
			rg2d.velocity = new Vector2 (rg2d.velocity.x, 5f);
			anim.SetTrigger("w");
		}
		
		if (Input.GetKeyDown(KeyCode.J)) {
			anim.SetTrigger("j");
		}
		
		if (Input.GetKeyDown(KeyCode.K)) {
			anim.SetTrigger("k");
		}
		
		
		
		
           

	}
	
}
