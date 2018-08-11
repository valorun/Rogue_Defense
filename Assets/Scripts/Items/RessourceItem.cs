using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceItem : Item {

	public RessourcesManager.SerializedRessource ressource;
	public override bool collect(){
		PopupFloatingText.instance.ShowMessage ("+"+ressource.value, transform, RessourcesManager.getRessourceColor(ressource.name));
		//GameManager.playerInstance.GetComponent<Player>().getRessources ()[ressource.name]+=ressource.value;
		GameManager.playerInstance.GetComponent<Player>().gainRessource (ressource.name, ressource.value);
		MapManager.instance.resetPosition (gameObject.transform.position);
		return true;
		//Destroy(gameObject);
	}
}
