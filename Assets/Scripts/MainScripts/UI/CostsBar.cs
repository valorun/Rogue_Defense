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
	// Update is called once per frame
	public void updateDisplay(){
		GameObject building = GameManager.playerInstance.GetComponent<Player> ().getBuildingToPlace ();
		GameObject selection = GameManager.playerInstance.GetComponent<Player> ().getSelectedTile ();
		GameObject item = GameManager.playerInstance.GetComponent<Player> ().getItemInSlot ();
		if (building != null && building.GetComponent<Building> () != null) {
			show ();
			buildingNameText.text = building.GetComponent<Building> ().getName ();
			RessourcesManager.SerializedRessource[] costs = building.GetComponent<Building> ().getCosts ();

			if (selection != null && item != null) {
				string upgradeType = item.GetComponent<UsableItem> ().getType ();
				if (building.GetComponent<Building> ().hasUpgrade (upgradeType)) {
					costs = building.GetComponent<Building> ().getUpgradeCosts (upgradeType);
				}
			}
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
