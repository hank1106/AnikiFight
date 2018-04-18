using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameStart : MonoBehaviour {

	// Use this for initialization
	public void load(string name){
		print("hit");
		SceneManager.LoadScene(name);
		
	}
}
