using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem : Item {

	public Sprite icon;
	protected Player player;
	public override bool collect(Player player){
		this.player = player;
		UsableItem playerItemSlot=player.takeItem (this); //return the old item
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
	public bool drop(){
		if(player == null)
			return false;
		else{
			Vector2 pos = (Vector2)player.transform.position;
			GameObject objAtPos = MapManager.instance.getObjectAt((int)pos.x, (int)pos.y);
			if(objAtPos == null){
				MapManager.instance.placeObjectAt (this.gameObject, (Vector2)transform.position);
				this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
				this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
			}
			else
				return false;
		}
		return true;
	} 
	public abstract bool activate();

	public abstract bool isActivable();

	public Sprite getIcon(){
		return icon;
	}
}