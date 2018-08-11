using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupFloatingText : MonoBehaviour {

	public GameObject guiTextContainer;
	public Canvas canvas;
	public float delay;
	public static PopupFloatingText instance = null;
	// Use this for initialization
	void Awake () {
		if (instance == null)instance = this;
	}

	public void ShowMessage (string message, Transform pos, Color32 color) {
		Camera playerCam = GameManager.playerInstance.GetComponent<Player> ().getCamera ();
		Vector2 screenPos = playerCam.WorldToScreenPoint (new Vector2 (pos.position.x + Random.Range (-0.5f, 0.5f), pos.position.y+0.5f));
		GameObject tempTextContainer= Instantiate (guiTextContainer) as GameObject;
		tempTextContainer.transform.SetParent (canvas.transform, false);
		tempTextContainer.transform.position = screenPos;

		Text tempText = tempTextContainer.transform.Find ("PopupText").GetComponent<Text> ();
		tempText.text = message;
		tempText.color = color;
		Destroy (tempTextContainer, delay);
	}
}
