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
	public void updateDisplay (Dictionary<string, int> ressources) {
		copperText.text = "" + ressources["copper"];
		ironText.text = "" + ressources["iron"];
		coalText.text = "" + ressources["coal"];
		uraniumText.text = "" + ressources["uranium"];
		goldText.text = "" + ressources["gold"];
	}
}
