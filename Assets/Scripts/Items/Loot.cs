using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {
	
	public GameObject[] commonsLoots;
	public GameObject[] raresLoots;
	public GameObject[] epicsLoots;
	// Use this for initialization
	public Dictionary<GameObject, float> lootList;

	public void spawnLoot(){
		int lootRNG = Random.Range(0, 100);
		GameObject toInstantiate=null;
		if(lootRNG > 70 && lootRNG < 90 && commonsLoots.Length>0){
			toInstantiate = commonsLoots[Random.Range(0, commonsLoots.Length)];
		}
		else if(lootRNG > 90 && lootRNG < 99 && raresLoots.Length>0){
			toInstantiate = raresLoots[Random.Range(0, raresLoots.Length)];
		}
		else if (lootRNG > 99 && epicsLoots.Length>0){
			toInstantiate = epicsLoots[Random.Range(0, epicsLoots.Length)];
		}
		if (toInstantiate != null)
			MapManager.instance.placeObjectAt (toInstantiate, transform.position);//Instantiate(toInstantiate, transform.position, transform.rotation);
	}
}
