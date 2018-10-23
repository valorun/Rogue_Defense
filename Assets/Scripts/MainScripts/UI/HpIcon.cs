using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpIcon : MonoBehaviour {

	public Text playerHp;
	Animator anim;
	// Use this for initialization
	void Awake () {
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	public void updateDisplay (int hp) {
		playerHp.text = "" + hp;
		anim.SetTrigger ("changed");
	}
}
