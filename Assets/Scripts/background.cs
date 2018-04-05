using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC;

public class background : MonoBehaviour {
	
	public static Animator anim;
	private SpriteRenderer mySpriteRenderer;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
