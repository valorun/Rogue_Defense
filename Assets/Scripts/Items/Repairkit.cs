using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairkit : UsableItem {
    public int value;

    public override bool activate(){
		if (player != null) {
            Building selection = player.getSelectedTile();
            if(selection != null){
				if (selection.getMissingHp() > 0) {
                    selection.heal(value);
					return true;
				}
				else PopupMessage.instance.ShowMessage ("This building has been already repaired");
            }
		} 
		else PopupMessage.instance.ShowMessage ("No building selected");
		return false;
	}
    public override bool isActivable(){
		if (player != null) {
            Building selection = player.getSelectedTile();
            if(selection != null){
				return selection.getMissingHp() > 0;
            }
		} 
		return false;
    }
    
	public int getValue(){
		return value;
	}
}