using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
	public Text ComboText;
	public Text gameOverText; //A reference to the UI text component that displays the player's score.
	public Button restart;
	public Button menu;

	public static GameControl instance;         //A reference to our game control script so we can access it statically.
	public int gameOver;
	private int Combo;
	
	void Awake()
	{
		//If we don't currently have a game control...
		if (instance == null)
			//...set this one to be it...
			instance = this;
		//...otherwise...
		else if(instance != this)
			//...destroy this one because it is a duplicate.
			Destroy (gameObject);
	}
	void Start () {
		Combo = 0;
		gameOver = 0;

		gameOverText.text = "";

	}

	// Update is called once per frame
	void Update () {
		if (gameOver == 1) {
			gameOverText.text = "Game Over" ;
		} else if(gameOver == 2 ){
			gameOverText.text = "You Win!";
			gameOverText.color = Color.red;
		}
	}
	public void Score(){
		Combo ++;
		if (Combo > 1) {
			ComboText.text = Combo.ToString() + "HITS!";
		}

	}
	
	public void ComboFail() {
		Combo = 0;
	}
	public void PlayerDead() {
		gameOver = 1;	
	}
	
	public void AIDead() {
		gameOver = 2;	
	}

}
