using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager : MonoBehaviour {
	[System.Serializable]
	public struct SerializedRessource {
		public string name;
		public int value;
	}

	public static SerializedRessource[] getRessourceList(){
		Dictionary<string, int> ressources = getRessourcesDictionnary ();
		SerializedRessource[] ressourceList=new SerializedRessource[ressources.Count];
		int index = 0;
		foreach(KeyValuePair<string,int> r in ressources){
			ressourceList [index].name = r.Key;
			ressourceList [index].value = r.Value;
			index++;
		}
		return ressourceList;
	}

	public static Dictionary<string, int> getRessourcesDictionnary (){
		Dictionary<string, int> ressources=new Dictionary<string,int>();
		ressources.Add ("gold",0);
		ressources.Add ("copper",0);
		ressources.Add ("iron",0);
		ressources.Add ("coal",0);
		ressources.Add ("uranium",0);
		return ressources;
	}
	public static Dictionary<string, int> getRessourcesDictionnary (int i){
		Dictionary<string, int> ressources=new Dictionary<string,int>();
		ressources.Add ("gold",0);
		ressources.Add ("copper",i);
		ressources.Add ("iron",i);
		ressources.Add ("coal",i);
		ressources.Add ("uranium",i);
		return ressources;
	}
	public static Color32 getRessourceColor(string ressource){
		switch (ressource) {
		case "gold":
			return new Color32 (233,216,0,0);
			break;
		case "copper":
			return new Color32 (208,88,29,0);
			break;
		case "iron":
			return new Color32 (156,156,156,0);
			break;
		case "coal":
			return new Color32 (38,38,38,0);
			break;
		case "uranium":
			return new Color32 (0,183,0,0);
			break;
		}
		return  new Color32 (0,0,0,0);
	}
}
