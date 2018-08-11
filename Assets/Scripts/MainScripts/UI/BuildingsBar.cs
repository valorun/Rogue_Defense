using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsBar : MonoBehaviour {

	bool isActive;
	Animator anim;
	void Awake(){
		anim = gameObject.GetComponent<Animator> ();
		show ();
	}
	void OnEnable() {
		show ();
	}
	public void selectBuilding(GameObject building){
		GameManager.playerInstance.GetComponent<Player>().placeBuilding(building);
	}
	public void updateDisplay(){
		GameObject selection = GameManager.playerInstance.GetComponent<Player> ().getSelectedTile ();
		if (!isActive && selection == null) {
			show ();
		} else if (isActive && selection != null)
			hide ();
	}
	void hide(){
		anim.SetBool ("isActive", false);
		isActive = false;
	}
	void show(){
		anim.SetBool ("isActive", true);
		isActive = true;
	}
}
