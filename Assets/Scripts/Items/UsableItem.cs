using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : Item {

	public Sprite icon;
	public string type;

	public override bool collect(){
		//GameManager.playerInstance.GetComponent<Player>().takeItem(gameObject);
		GameObject playerItemSlot=GameManager.playerInstance.GetComponent<Player> ().takeItem (gameObject);
		if (playerItemSlot == null) {
			MapManager.instance.clearPosition (gameObject.transform.position);
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			return true;
		} else {
			MapManager.instance.replaceObjectAt (playerItemSlot, (Vector2)transform.position);
			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			playerItemSlot.GetComponent<SpriteRenderer> ().enabled = true;
			playerItemSlot.GetComponent<BoxCollider2D> ().enabled = true;
			return true;
		}
		//StartCoroutine (PopupMessage.instance.ShowMessage("You already have an object", 2f));
		return false;
	}
	public bool activate(){
		if (getPlayerSelectedTile () != null) {
			if (getPlayerSelectedTile ().GetComponent<Building> () != null) {
				if (getPlayerSelectedTile ().GetComponent<Building> ().hasUpgrade (type)) {
					bool result = getPlayerSelectedTile ().GetComponent<Building> ().upgrade (type);
					if (!result)
						 PopupMessage.instance.ShowMessage ("Not enough ressources");
					return result;
				}
				else PopupMessage.instance.ShowMessage ("Item cannot be used on this building");
			}
			else PopupMessage.instance.ShowMessage ("Item must be used on a building");
		} 
		else PopupMessage.instance.ShowMessage ("No building selected");
		return false;
	}
	public Sprite getIcon(){
		return icon;
	}
	public string getType(){
		return type;
	}
	GameObject getPlayerSelectedTile(){
		return GameManager.playerInstance.GetComponent<Player> ().getSelectedTile ();
	}
}