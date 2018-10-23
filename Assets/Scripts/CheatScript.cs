using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour {

	// Update is called once per frame
	private Player player;

	void Start(){
		player = GameManager.playerInstance;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.G))player.gainRessource ("gold", 100);
		if(Input.GetKeyDown(KeyCode.H))player.gainRessource ("copper", 100);
		if(Input.GetKeyDown(KeyCode.J))player.gainRessource ("iron", 100);
		if(Input.GetKeyDown(KeyCode.K))player.gainRessource ("coal", 100);
		if(Input.GetKeyDown(KeyCode.L))player.gainRessource ("uranium", 100);
	}
}
