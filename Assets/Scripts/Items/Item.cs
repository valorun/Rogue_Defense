using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
	public string itemName;
	public abstract bool collect (Player player);

	public string getName(){
		return itemName;
	}
}
