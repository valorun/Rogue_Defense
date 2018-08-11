using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour {

	public Text guiText;
	public float displayTime=2f;
	public static PopupMessage instance = null;
	// Use this for initialization
	void Awake () {
		if (instance == null)instance = this;
	}

	IEnumerator ShowMessageIE (string message) {
		guiText.text = message;
		guiText.enabled = true;
		yield return new WaitForSeconds(displayTime);
		guiText.enabled = false;
	}
	public void ShowMessage(string message){
		StartCoroutine ("ShowMessageIE", message);
	}
}
