using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostsBar : MonoBehaviour {

	public Text buildingNameText;
	public Text copperCostText;
	public Text ironCostText;
	public Text coalCostText;
	public Text uraniumCostText;
	public Text goldCostText;

	Animator anim;

	void Awake(){
		anim = gameObject.GetComponent<Animator> ();
		anim.speed = 3;
	}
	void OnEnable() {
		show ();
	}

	public void updateDisplay(Building building){
		if (building != null) {
			show ();
			buildingNameText.text = building.getName ();
			RessourcesManager.SerializedRessource[] costs = building.getCosts ();

			foreach (RessourcesManager.SerializedRessource r in costs) {
				if (r.name == "copper") {
					copperCostText.text = "" + r.value;
				} else if (r.name == "iron") {
					ironCostText.text = "" + r.value;
				} else if (r.name == "coal") {
					coalCostText.text = "" + r.value;
				} else if (r.name == "uranium") {
					uraniumCostText.text = "" + r.value;
				} else if (r.name == "gold") {
					goldCostText.text = "" + r.value;
				}
			}
		} else
			hide ();
	}
	void hide(){
		anim.SetBool ("isActive", false);
	}
	void show(){
		anim.SetBool ("isActive", true);
	}
}
