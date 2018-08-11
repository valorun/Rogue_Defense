using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RessourcesBar : MonoBehaviour {

	public Text copperText;
	public Text ironText;
	public Text coalText;
	public Text uraniumText;
	public Text goldText;
	
	// Update is called once per frame
	public void updateDisplay () {
		copperText.text = "" + getPlayerRessource("copper");
		ironText.text = "" + getPlayerRessource("iron");
		coalText.text = "" + getPlayerRessource("coal");
		uraniumText.text = "" + getPlayerRessource("uranium");
		goldText.text = "" + getPlayerRessource("gold");
	}
	int getPlayerRessource(string ressource){
		return GameManager.playerInstance.GetComponent<Player> ().getRessources () [ressource];
	}
}
