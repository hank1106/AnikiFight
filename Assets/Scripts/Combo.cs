using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Combo : NetworkBehaviour {
	private int currentATK = 0;
	private float preBlood1 = 8.05f;
	private float preBlood2 = 8.05f;
	private GameObject bloodBar1;
	private GameObject bloodBar2;
	private int counter = 0;
	public Text ComboText;
	public Text gameOverText;
	// Use this for initialization
	void Start () {
		ComboText = GameObject.Find("Combo").GetComponent<Text>();
		gameOverText = GameObject.Find("GameOver").GetComponent<Text>();
		bloodBar1 = GameObject.Find ("Blood1");
		bloodBar2 = GameObject.Find ("Blood2");
	}

	void OnDestroy(){
		ComboText.text = "";
		if(bloodBar1!=null)bloodBar1.transform.localScale = new Vector3(8.05f, 0.34f, 0);
		if(bloodBar2!=null)bloodBar2.transform.localScale = new Vector3(8.05f, 0.34f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		float curBlood1 = bloodBar1.transform.localScale.x;
		float curBlood2 = bloodBar2.transform.localScale.x;
		if (curBlood1 != preBlood1) {
			preBlood1 = curBlood1;
			if (currentATK != 2) {
				currentATK = 2;
				counter = 0;
				ComboText.text = "";
			}
			counter++;
			if(counter>1) ComboText.text = counter.ToString() + "HITS!";
		} else if (curBlood2 != preBlood2) {
			preBlood2 = curBlood2;
			if (currentATK != 1) {
				currentATK = 1;
				counter = 0;
				ComboText.text = "";
			}
			counter++;
			if(counter>1) ComboText.text = counter.ToString() + "HITS!";
		} 
	}
}
