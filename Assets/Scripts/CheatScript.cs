using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.G))GameManager.playerInstance.GetComponent<Player> ().gainRessource ("gold", 100);
		if(Input.GetKeyDown(KeyCode.H))GameManager.playerInstance.GetComponent<Player> ().gainRessource ("copper", 100);
		if(Input.GetKeyDown(KeyCode.J))GameManager.playerInstance.GetComponent<Player> ().gainRessource ("iron", 100);
		if(Input.GetKeyDown(KeyCode.K))GameManager.playerInstance.GetComponent<Player> ().gainRessource ("coal", 100);
		if(Input.GetKeyDown(KeyCode.L))GameManager.playerInstance.GetComponent<Player> ().gainRessource ("uranium", 100);
	}
}
