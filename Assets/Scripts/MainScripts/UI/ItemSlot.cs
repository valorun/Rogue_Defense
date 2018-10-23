using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	
	public Image itemSlotIcon;
	public Text itemSlotText;

	UsableItem item;
	void Start () {
		itemSlotText.text = "";
		//itemSlotIcon = null;
	}

	public void setItem(UsableItem item){
		this.item = item;
		if (item != null) {
			itemSlotIcon.sprite = item.getIcon ();
			itemSlotText.text = item.getName();
			itemSlotIcon.enabled = true;
			updateIconColor();
		} else {
			itemSlotIcon.sprite = null;
			itemSlotText.text = "";
			itemSlotIcon.enabled = false;
		}
	}

	public void updateIconColor(){
		//Building building = GameManager.playerInstance.GetComponent<Player> ().getBuildingToPlace ();
		//Building selection = player.getSelectedTile ();
		//UsableItem item = player.getItemInSlot ();
		if(item != null){
			if (item.isActivable())
				itemSlotIcon.color = new Color32 (255, 255, 255, 255);
			else
				itemSlotIcon.color = new Color32 (180, 0, 0, 100);
		}
	}
	public void useItem(){
		if (item!=null && item.activate ()) {
			setItem(null);
			updateIconColor ();
		} 
	}
}
