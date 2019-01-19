using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : UsableItem {
	public int value;

    	public override bool activate(){
		if (isActivable()) {
            	player.heal(value);
		} 
		else PopupMessage.instance.ShowMessage ("Player is already full life");
		return false;
	}
    	public override bool isActivable(){
		if (player != null && player.getMissingHp() > 0) {
            	return true;
		} 
		return false;
    	}

	public int getValue(){
		return value;
	}
}