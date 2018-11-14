﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBar : MonoBehaviour {

	public Text selectionHp;
	public GameObject powBuildingSelection;
	public Text selectionPower;
	public GameObject generatorSelection;
	public Text selectionProducedPower;
	public GameObject healerSelection;
	public Text selectionHealingPower;
	bool isActive;
	Animator anim;

	// Update is called once per frame
	void Start(){
		anim = gameObject.GetComponent<Animator> ();
		hide ();
	}
	public void updateDisplay (Building selection) {
		if (selection != null && selection.GetComponent<Building> () != null) {
			if(!isActive)show ();
			setHpDisplay(selection.GetComponent<Building> ().getHp ());

			if (selection != null && selection.GetComponent<PoweredBuilding> () != null) {
				powBuildingSelection.SetActive (true);
				selectionPower.text = "" + selection.GetComponent<PoweredBuilding> ().getPower () + "/" + selection.GetComponent<PoweredBuilding> ().getMaxPower ();
			} else
				powBuildingSelection.SetActive (false);
			if (selection != null && selection.GetComponent<Generator> () != null) {
				generatorSelection.SetActive (true);
				selectionProducedPower.text = "" + selection.GetComponent<Generator> ().getProducedPower ();
			} else
				generatorSelection.SetActive (false);
			if (selection != null && selection.GetComponent<Healer> () != null) {
				healerSelection.SetActive (true);
				selectionHealingPower.text = "" + selection.GetComponent<Healer> ().getHealingPower ();
			} else
				healerSelection.SetActive (false);
		} else
			if(isActive)hide ();
	}

	public void setHpDisplay(int hp){
		selectionHp.text = "" + hp;
	}

	public void sellBuilding(){
		GameManager.playerInstance.sellSelection();
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
