using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : UsableItem {
    public int value;

    public override bool activate(){
		if (isActivable()) {
            player.heal(value);
		} 
		else PopupMessage.instance.ShowMessage ("No building selected");
		return false;
	}
    public override bool isActivable(){
		if (player != null) {
            return true;
		} 
		return false;
    }

	public int getValue(){
		return value;
	}
}