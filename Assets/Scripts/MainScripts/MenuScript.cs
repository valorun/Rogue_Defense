using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas mainMenu;
	public Canvas levelSelectionMenu;
	public Canvas helpMenu;
	public Text startButtonText;
	public Text highScoreText;
	public static int selectedMapSize=1;
	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt ("actualWave",1)==1)startButtonText.text="New game";
		else startButtonText.text="Continue";
		if (PlayerPrefs.GetInt ("bestWave", 1) > 1)
			highScoreText.text = "Best score: " + PlayerPrefs.GetInt ("bestWave", 1) + " waves";
		else
			highScoreText.text = "";
		displayMainMenu ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))Application.Quit();
	}

	public void Quit(){
		Application.Quit ();
	}

	public void displayLevelSelectionMenu(){
		if (PlayerPrefs.GetInt ("actualWave", 1) > 1) {
			PlayerPrefs.GetInt ("actualMapSize");
			SceneManager.LoadScene("Loading");
		} else {
			mainMenu.enabled = false;
			helpMenu.enabled = false;
			levelSelectionMenu.enabled = true;
		}
	}
	public void displayMainMenu(){
		levelSelectionMenu.enabled=false;
		helpMenu.enabled=false;
		mainMenu.enabled=true;
	}
	public void displayHelpMenu(){
		levelSelectionMenu.enabled=false;
		mainMenu.enabled=false;
		helpMenu.enabled=true;
	}
	public void playSelectedLevel(int size){
		selectedMapSize = size;
		SceneManager.LoadScene("Loading");
	}
}
