using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceItem : Item {

	public RessourcesManager.SerializedRessource ressource;
	public override bool collect(Player player){
		PopupFloatingText.instance.ShowMessage ("+"+ressource.value, transform, RessourcesManager.getRessourceColor(ressource.name));
		player.gainRessource (ressource.name, ressource.value);
		MapManager.instance.resetPosition (gameObject.transform.position);
		return true;
		//Destroy(gameObject);
	}
}
