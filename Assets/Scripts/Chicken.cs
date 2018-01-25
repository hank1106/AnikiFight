using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Chicken : MonoBehaviour {

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

	public void Die() {

		arrAllAudioSource[1].Play();
		anim.SetTrigger("Die");
		rg2d.velocity = Vector2.zero;
		rg2d.isKinematic = true;
		CircleCollider2D collider = GetComponent<CircleCollider2D>();
		collider.enabled = false;
		Destroy (collider);
		GameControl.instance.CountDead ();

	}

	private float force  = 3.5f;
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
	}
	void OnCollisionEnter2D( Collision2D col){
		if (col.gameObject.layer == 8 || col.gameObject.layer == 10) {
			rg2d.velocity = new Vector2 (force, 0);
		} else if (col.gameObject.layer == 9) {
			rg2d.velocity = new Vector2 (-force, 0);
		} else if (col.gameObject.name.Contains ("basket")) {
			print ("DDDDDDDDDDD");
			/*
			rg2d.velocity = Vector2.zero;
			rg2d.isKinematic = true;
			CircleCollider2D collider = GetComponent<CircleCollider2D>();
			collider.enabled = false;
			Destroy (collider);
			arrAllAudioSource[2].Play();
			*/
		} else if (col.gameObject.name.Contains ("Ground")) {
			arrAllAudioSource[1].Play();
			anim.SetTrigger("Die");
			rg2d.velocity = Vector2.zero;
			rg2d.isKinematic = true;
			CircleCollider2D collider = GetComponent<CircleCollider2D>();
			collider.enabled = false;
			Destroy (collider);
			GameControl.instance.CountDead ();
		} else if (col.gameObject.name.Contains ("bigstick")) {
			arrAllAudioSource[1].Play();
			anim.SetTrigger("Die");
			rg2d.velocity = Vector2.zero;
			rg2d.isKinematic = true;
			CircleCollider2D collider = GetComponent<CircleCollider2D>();
			collider.enabled = false;
			Destroy (collider);
			GameControl.instance.CountDead ();
		} else if (col.gameObject.name.Contains ("spring")) {
			rg2d.velocity = new Vector2 (force, 2f);
			print ("speed: " + rg2d.velocity.x);
			arrAllAudioSource[0].Play();

		} else if (col.gameObject.layer == 11) {
			//to right
			rg2d.velocity = new Vector2 (3f, 0);

		}
		else if (col.gameObject.layer == 12) {
			rg2d.velocity = new Vector2 (-3f, 0);

		}
		float ppp = rg2d.velocity.x;
		print (ppp);
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.GetComponents<Chicken>() != null && other.name == "basket") {
			print ("basket" );
			rg2d.velocity = Vector2.zero;
			rg2d.isKinematic = true;
			CircleCollider2D collider = GetComponent<CircleCollider2D>();
			collider.enabled = false;
			Destroy (collider);
			arrAllAudioSource[2].Play();
			GameControl.instance.Score ();
		}else if(other.GetComponents<Chicken>() != null && other.name.Contains("Waterfall")){
			print ("waterfall" );
			rg2d.velocity = new Vector2 (rg2d.velocity.x * 0.05f, 0);
		}else if(other.GetComponents<Chicken>() != null && other.name == "collider"){
			rg2d.transform.localScale = new Vector2(-rg2d.transform.localScale.x, rg2d.transform.localScale.y);
		}
	}
	public void changeDirection(){
		force = -force;
	}
}
