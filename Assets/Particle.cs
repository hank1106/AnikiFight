using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystemRenderer>().sortingLayerName = "playerLayer";
		GetComponent<ParticleSystemRenderer>().sortingOrder = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
