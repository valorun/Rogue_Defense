using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : Item {

	public Sprite icon;
	protected Player player;
	public override bool collect(Player player){
		this.player = player;
		UsableItem playerItemSlot=player.takeItem (this);
		gameObject.GetComponent<SpriteRenderer> ().enabled = false; //hide the physical object
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		if (playerItemSlot == null) {
			MapManager.instance.clearPosition (gameObject.transform.position);
		} else {
			MapManager.instance.replaceObjectAt (playerItemSlot.gameObject, (Vector2)transform.position); //place the old item to this item position
			playerItemSlot.GetComponent<SpriteRenderer> ().enabled = true;
			playerItemSlot.GetComponent<BoxCollider2D> ().enabled = true;
			return true;
		}
		//StartCoroutine (PopupMessage.instance.ShowMessage("You already have an object", 2f));
		return true;
	}
	public abstract bool activate();

	public abstract bool isActivable();
	public Sprite getIcon(){
		return icon;
	}
}