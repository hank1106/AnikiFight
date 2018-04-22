using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NiisanCombo : NetworkBehaviour {
	private static int currentATK = 0;
	private static float preBlood1 = 8.05f;
	private static float preBlood2 = 8.05f;
	private Slider bloodBar1;
	private Slider bloodBar2;
	private Vector2 leftBarPosition;
	private Vector2 rightBarPosition;
	//private RectTransform bloodBar1;
	//private RectTransform bloodBar2;

	private static int counter = 0;
	public GameObject Combo;
	public Text ComboText;
	public Text gameOverText;
	// Use this for initialization
	void Start () {
		Combo = GameObject.Find ("Combo");
		ComboText = Combo.GetComponent<Text>();
		gameOverText = GameObject.Find("GameOver").GetComponent<Text>();
		bloodBar1 = GameObject.Find("BloodBarLeft").GetComponent<Slider>();
		bloodBar2 = GameObject.Find("BloodBarRight").GetComponent<Slider>();
		leftBarPosition = (Vector2) GameObject.Find ("BloodBarLeft").transform.position;
		rightBarPosition = (Vector2) GameObject.Find ("BloodBarRight").transform.position;
		//bloodBar1 = GameObject.Find("LeftForeground").GetComponent<RectTransform>();
		//bloodBar2 = GameObject.Find("RightForeground").GetComponent<RectTransform>();
	}

	void OnDestroy(){
		ComboText.text = "";
		if(bloodBar1!=null)bloodBar1.value = 0;
		if(bloodBar2!=null)bloodBar2.value = 0;
		//if(bloodBar1!=null) bloodBar1.sizeDelta = new Vector2(0, bloodBar1.sizeDelta.y);
		//if(bloodBar2!=null) bloodBar2.sizeDelta = new Vector2(0, bloodBar2.sizeDelta.y);
	}

	public void GameOver(bool status){
		if (status)
			gameOverText.text = "You Win!";
		else
			gameOverText.text = "You Lose!";
		gameOverText.transform.localScale = new Vector2 (6f, 6f);
		ComboText.text = "";
	}

	public void CalcCombo () {
		CmdCalcCombo ();
		// counter++;
		// if(counter>1) CmdComboShow();
		/*
		float curBlood1 = bloodBar1.value;
		float curBlood2 = bloodBar2.value;
		if (curBlood1 != preBlood1) {
			preBlood1 = curBlood1;
			if (currentATK != 2) {
				currentATK = 2;
				counter = 0;
				CmdComboClear ();
			}
			counter++;
			if(counter>1) CmdComboShow();
		} else if (curBlood2 != preBlood2) {
			preBlood2 = curBlood2;
			if (currentATK != 1) {
				currentATK = 1;
				counter = 0;
				CmdComboClear ();
			}
			counter++;
			if(counter>1) CmdComboShow();
		} */
	}

	[ClientRpc]
	void RpcCalcCombo(){
		float curBlood1 = bloodBar1.value;
		float curBlood2 = bloodBar2.value;
		//float curBlood1 = bloodBar1.sizeDelta.x/2f;
		//float curBlood2 = bloodBar2.sizeDelta.x/2f;

		if (curBlood1 != preBlood1) {
			preBlood1 = curBlood1;
			if (currentATK != 2) {
				//Debug.Log ("Attacker from 1 to 2");
				counter = 0;
				ComboText.text = "";
			}
			currentATK = 2;
			counter++;
			if(counter>1) ComboText.text = counter.ToString() + "HITS!";
			Combo.transform.position = new Vector2 (leftBarPosition.x, ComboText.transform.position.y);
		} else if (curBlood2 != preBlood2) {
			preBlood2 = curBlood2;
			if (currentATK != 1) {
				//Debug.Log ("Attacker from 2 to 1");
				counter = 0;
				ComboText.text = "";
			}
			currentATK = 1;
			counter++;
			if(counter>1) ComboText.text = counter.ToString() + "HITS!";
			Combo.transform.position = new Vector2 (rightBarPosition.x, ComboText.transform.position.y);
		}
	}

	/*
	[ClientRpc]
	void RpcComboClear(){
		ComboText.text = "";
	}

	[ClientRpc]
	void RpcComboShow(){
		ComboText.text = ComboText.text = counter.ToString() + "HITS!";
	}
	*/

	[Command]
	void CmdCalcCombo(){
		RpcCalcCombo ();
	}

	/*
	[Command]
	void CmdComboClear(){
		RpcComboClear ();
	}

	[Command]
	void CmdComboShow(){
		RpcComboShow ();
	}
	*/
}
