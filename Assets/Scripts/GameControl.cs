using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour {
	//public Text scoreText;
	public Text gameOverText; //A reference to the UI text component that displays the player's score.
	public Text lifeCount;
	public Button pause;
	public Sprite continueImage;
	public Sprite pasueImage;

	public Button menu;
	public Button next;
	public Button restart;
	public Button menuInGame;
	public static GameControl instance;         //A reference to our game control script so we can access it statically.
	public int chickenDead;
	public bool gameOver;
	private int score;
	private int maxDeath;
	public GameObject background;
	public GameObject scores;
	public GameObject noscore;
	public GameObject egg1;
	public GameObject egg2;
	public GameObject egg3;

	public bool isWin;


	// Use this for initialization
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
		isWin = false;
		score = 0;
		chickenDead = 0;
		gameOver = false;
		maxDeath = 6;
		gameOverText.text = "";
		pause.onClick.AddListener (Pause);
		menu.onClick.AddListener (Menu);
		menuInGame.onClick.AddListener (MenuInGame);
		menu.gameObject.SetActive (false);
		next.gameObject.SetActive (false);
		next.onClick.AddListener (Next);
		restart.gameObject.SetActive (false);
		restart.onClick.AddListener (Restart);
		background.SetActive(false);
		scores.SetActive(false);
		noscore.SetActive(false);
		egg1.SetActive(false);
		egg2.SetActive(false);
		egg3.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		lifeCount.text = "X " + (6 - chickenDead - score).ToString ();

		if (gameOver == true) {
			gameOverText.text = "\n \n \n Level Failed \n Try again!" ;
			restart.gameObject.SetActive (true);
			menu.gameObject.SetActive (true);
			next.gameObject.SetActive (true);
			background.SetActive(true);
			noscore.SetActive (true);
			pause.gameObject.SetActive (false);

		} else if((score + chickenDead) == 6  && score != 0 && !isWin){
			isWin = true;
			print ("Win");
			gameOverText.text = "You Win!";
			//gameOverText.color = Color.red;
			restart.gameObject.SetActive (true);
			menu.gameObject.SetActive (true);
			next.gameObject.SetActive (true);
			background.SetActive(true);
			scores.SetActive(true);
			pause.gameObject.SetActive (false);
			string name= SceneManager.GetActiveScene().name;
			int level = int.Parse(name[name.Length-1].ToString());
			if(score == 1  ){
				egg1.SetActive (true);
				gameOverText.text = "Good!";
				SaveGame (level,1);
			}
			if(score > 1 && score < 6 ){
				egg1.SetActive (true);
				egg2.SetActive (true);
				gameOverText.text = "Very Nice!";
				SaveGame (level,2);

			}if (score == 6) {
				egg1.SetActive (true);
				egg2.SetActive (true);
				egg3.SetActive(true);
				gameOverText.text = "Perfect!";
				SaveGame (level,3);
			}
		}
	}
	public void Score(){
		score++;
		//scoreText.text = "Score: " + score.ToString();

	}

	public void CountDead(){
		chickenDead++;
		if (chickenDead == maxDeath) {
			gameOver = true;
		}
	}

	void Pause(){
		print ("restart");
		if (Time.timeScale != 0) {
			print ("pause");
			Time.timeScale = 0;
			pause.image.sprite = continueImage;
		} else {
			print ("resume");

			Time.timeScale = 1;
			pause.image.sprite = pasueImage;
		}

	}

	void Menu(){
		SceneManager.LoadScene("menu");
	}

	void MenuInGame(){
		SceneManager.LoadScene ("menu");
	}

	void Next(){
		Scene current = SceneManager.GetActiveScene ();
		if (current.name.Equals ("Level1")) {
			SceneManager.LoadScene(current.buildIndex - 2);
		} else {
			SceneManager.LoadScene(current.buildIndex - 1);
		}
	}

	void Restart(){
		print ("restart");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void SaveGame(int level, int score)
	{
		//if (File.)
		//if (this.mutex > 0) return;
		//this.mutex = 1;
		print("open");
		//Load current save data.
		SaveData saveData = GameManager.instance.LoadSaveData();
		FileStream file = File.Open(Application.persistentDataPath + "/saves.dat", FileMode.OpenOrCreate);
		Dictionary<int, int> scores = saveData.getScores();
		if (saveData.scores.ContainsKey(level))
		{
			saveData.scores[level] = score;
		}
		else
		{
			saveData.scores.Add(level, score);
		}
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, saveData);
		file.Close ();
		//this.mutex = 0;
	}
}
