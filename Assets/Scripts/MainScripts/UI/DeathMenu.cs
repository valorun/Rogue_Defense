using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour {

	public Text deathScoreText;
	public void display(){
		//if (GameManager.playerInstance.GetComponent<Player> ().isDead()) {
			gameObject.GetComponent<Canvas>().enabled = true;
			deathScoreText.text = "";
			if (PlayerPrefs.GetInt ("bestWave", 1) < GameManager.instance.getWave ())
				deathScoreText.text = "Congratulation ! You reached a new Record.\n";
			deathScoreText.text += "Score of " + GameManager.instance.getWave ()+ " waves.";

		//} else {
			//gameObject.GetComponent<Canvas>().enabled = false;
		//}
	}
}
