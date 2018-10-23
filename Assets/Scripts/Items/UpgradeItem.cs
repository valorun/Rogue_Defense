using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItem : UsableItem {
    public string type;

    public override bool activate(){
		if (player != null) {
            Building selection = player.getSelectedTile();
            if(selection != null){
				if (selection.hasUpgrade (type)) {
					bool result = selection.upgrade (type);
					RessourcesManager.SerializedRessource[] upgradeCosts = selection.getUpgradeCosts(type);
					if (!player.enoughRessources (upgradeCosts)) {
						PopupMessage.instance.ShowMessage ("Not enough ressources");
					}
                    else{
						GameObject tempUpgrade = selection.getUpgrade (type);
						foreach (RessourcesManager.SerializedRessource r in upgradeCosts) { // give back ressources of the building, because the upgrade will be buy after
							player.gainRessource (r.name, r.value);
						}
						selection.upgrade(type);
                        player.setItemInSlot(null);
			            player.deselection();
                    }
					return result;
				}
				else PopupMessage.instance.ShowMessage ("Item cannot be used on this building");
            }
		} 
		else PopupMessage.instance.ShowMessage ("No building selected");
		return false;
	}
    public override bool isActivable(){
		if (player != null) {
            Building selection = player.getSelectedTile();
            if(selection != null){
				if (selection.hasUpgrade (type)) {
					return player.enoughRessources (selection.getUpgradeCosts(type));
				}
            }
		} 
		return false;
    }

	public Building getBuildingUpgrade(Building building){
		return building.getUpgrade(type).GetComponent<Building>();
	}
    
	public string getType(){
		return type;
	}
}