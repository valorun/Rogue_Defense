using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	
	public Image itemSlotIcon;
	public Text itemSlotText;

	void Start () {
		itemSlotText.text = "";
		setIcon (null);
	}
	public void updateIconColor(){
		GameObject building = GameManager.playerInstance.GetComponent<Player> ().getBuildingToPlace ();
		GameObject selection = GameManager.playerInstance.GetComponent<Player> ().getSelectedTile ();
		GameObject item = GameManager.playerInstance.GetComponent<Player> ().getItemInSlot ();
		if (building != null && item != null && selection!=null) {
			string itemType=item.GetComponent<UsableItem> ().getType ();
			//GameObject upgradeBuilding=selection.GetComponent<Building>().getUpgrade(itemType);
			bool hasUpgrade = selection.GetComponent<Building> ().hasUpgrade (itemType);
			if (hasUpgrade) {
				if (GameManager.playerInstance.GetComponent<Player> ().enoughRessources (building.GetComponent<Building>().getUpgradeCosts(itemType)))
					itemSlotIcon.color = new Color32 (255, 255, 255, 255);
				else
					itemSlotIcon.color = new Color32 (180, 0, 0, 100);
			}
		}
		else itemSlotIcon.color= new Color32(180,0,0,100);
	}
	public void setIcon(GameObject item){
		if (item != null) {
			itemSlotIcon.sprite = item.GetComponent<UsableItem> ().getIcon ();
			itemSlotText.text = item.GetComponent<UsableItem> ().getType ();
			itemSlotIcon.enabled = true;
		} else {
			itemSlotIcon.sprite = null;
			itemSlotText.text = "";
			itemSlotIcon.enabled = false;
		}

	}
	public void useItem(){
		if (GameManager.playerInstance.GetComponent<Player> ().getItemInSlot ()!=null && GameManager.playerInstance.GetComponent<Player> ().getItemInSlot ().GetComponent<UsableItem> ().activate ()) {
			GameManager.playerInstance.GetComponent<Player> ().setItemInSlot(null);
			setIcon (null);
			GameManager.playerInstance.GetComponent<Player> ().setBuildingToPlace (null);
			updateIconColor ();
		} 
	}
}
