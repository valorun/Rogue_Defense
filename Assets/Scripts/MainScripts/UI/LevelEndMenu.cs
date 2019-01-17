using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndMenu : MonoBehaviour {

	public Text levelEndScoreText;

	public void display(){
		levelEndScoreText.text = "";
		if (PlayerPrefs.GetInt ("bestWave", 1) < GameManager.instance.getWave ())
			levelEndScoreText.text = "Congratulation ! You reached a new record.\n";
		levelEndScoreText.text += "Score of " + GameManager.instance.getWave ()+ " waves.";
		gameObject.GetComponent<Canvas>().enabled = true;
	}
}
