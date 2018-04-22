using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ani : MonoBehaviour {

	private float start_wating_time = 0;
	public AudioSource[] arrAllAudioSource;
	// Use this for initialization
	void Start ()
    {
		arrAllAudioSource = GetComponents<AudioSource>();
        transform.position = new Vector3(0, 10, -10);
		start_wating_time = Time.time;
    }
    
    void LateUpdate ()
    {
		if (transform.position.y > 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, -10);
		}
		
		if (Time.time - start_wating_time > 6) {
			arrAllAudioSource[0].Play();
			load("Start");
		}
		
		if (Time.time - start_wating_time > 7) {
			
		}
        //transform.position = player.transform.position + offset;
    }
	
	public void load(string name){
		SceneManager.LoadScene(name);
		
	}
}
