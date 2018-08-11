using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadindScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(loadLevel("Level1"));
	}

	IEnumerator loadLevel(string level)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
		while (!asyncLoad.isDone){
			yield return null;
		}
	}
}
