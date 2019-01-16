using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	private int columns;
	private int rows;
	public GameObject spawnTile;
	private int spawnTilesDimension; //dimension of spawn tiles
	public GameObject player; //player gameobject to place
	public GameObject[] wallTiles; //all different paterns
	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;

	public GameObject[] ironVeinTiles;
	private int ironVeinMaxCount;
	private int ironVeinMinCount;

	public GameObject[] coalVeinTiles;
	private int coalVeinMaxCount;
	private int coalVeinMinCount;

	public GameObject[] copperVeinTiles;
	private int copperVeinMaxCount;
	private int copperVeinMinCount;

	public GameObject[] uraniumVeinTiles;
	private int uraniumVeinMaxCount;
	private int uraniumVeinMinCount;

	private int wallMaxCount;
	private int wallMinCount;
	public static MapManager instance = null;

	private Transform boardHolder;	
	private Vector2[] spawnPos;

	GameObject[,] slotsGrid; //the grid slots, to check if there is something at a specific position
	float[,] pathsValuesGrid; //the grid priorities for AI pathfinding


	void Awake(){
		if (instance == null)instance = this;
	}

	// This method create the grid positions and setup the main board
	void initializeMap(){
		//positionsGrid.Clear ();

		slotsGrid=new GameObject[columns,rows];
		pathsValuesGrid=new float[columns,rows];

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				pathsValuesGrid [x, y] = 10f;
			}
		}

		boardHolder = new GameObject ("board").transform;
		//place outer walls
		//here the index is between -1 and columns+1 because the outer walls are outside the board
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if(x == -1 || x == columns || y == -1 || y == rows)toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 1), toInstantiate.transform.rotation) as GameObject; //obj.transform.rotation = default gameObjetct rotation rotation
				instance.transform.SetParent (boardHolder);
			}
		}

	}
	public GameObject getObjectAt(int x, int y){
		if(x>=0 && x<columns && y>=0 && y<rows)return slotsGrid [x, y];
		return null;
	}
	// Return true if the map is full and nothing more can be added on grid
	bool mapIsFull(){
		for (int x = 0; x < columns ; x++) {
			for (int y = 0; y < rows ; y++) {
				if (slotsGrid [x, y] == null)
					return false;
			}
		}
		return true;
	}
	public bool positionIsInsideMap(int x, int y){
		return (x < columns && x>=0 && y < rows && y>=0);
	}
	Vector2 randomPosition (){
		int randomX = Random.Range (0, columns);
		int randomY = Random.Range (0, rows);
		if(slotsGrid [randomX, randomY] != null && !mapIsFull()){
			return randomPosition();
		}
		else{
			//Debug.Log (gridSlots [randomX, randomY]==null);
			Vector2 randomPos = new Vector2(randomX, randomY);
			return randomPos;
		}
	}
	Vector2 randomSpawnPosition(){
		int random = Random.Range (0, spawnPos.Length);
		Debug.Log (spawnPos.ToString());
		return spawnPos [random];
	}
	public List<GameObject> getNearbyGameObjects(Vector2 pos, int radius){
		List<GameObject> nearbyGameObjects = new List<GameObject>();
		for (int x = -radius; x <= radius; x++){
			for (int y = -radius; y <= radius; y++){
				if (x == 0 && y == 0)
					continue;
				GameObject go = getObjectAt(x+(int)pos.x, y+(int)pos.y);
				if (go != null)
					nearbyGameObjects.Add(go);
				
			}
		}
		return nearbyGameObjects;
	} 
	public void placeObjectAt(GameObject obj, Vector2 pos){
		placeObjectAt(obj, (Vector3)pos);
	}
	public void placeObjectAt(GameObject obj, Vector3 pos){
		int x = (int)pos.x;
		int y = (int)pos.y;
		//int z = (int)pos.z;
		if (!mapIsFull () && slotsGrid [x, y] == null) {
			GameObject instance=Instantiate (obj, pos, obj.transform.rotation); //obj.transform.rotation = default gameObjetct rotation rotation
			if(obj.tag=="Player")slotsGrid [x, y]=null; //enemies and player don't take up space.
			else slotsGrid [x, y] = instance;
			updatePathValue (x, y);
			instance.transform.SetParent (boardHolder);
		}
	}
	public void replaceObjectAt(GameObject newObj, Vector2 pos){
		int x = (int)pos.x;
		int y = (int)pos.y;
		slotsGrid [x, y]=newObj;
		newObj.transform.position = pos;
		newObj.transform.SetParent (boardHolder);
	}
	void updatePathValue(int x, int y){
		float value = 10f;
		GameObject obj = slotsGrid [x,y];
		if (obj==null)
			value = 10f;
		else if(obj.tag == "Destructible")
			value = 0.0f;
		else if(obj.tag == "Building")
			value = 1.0f;
		pathsValuesGrid [x,y] = value;
	}
	public float[,] getPathsValuesGrid(){
		return pathsValuesGrid;
	}

	void placeObjectAtRandom(GameObject[] objectArray, int minCount, int maxCount){
		int objectCount = Random.Range (minCount, maxCount);
		for (int i = 0; i < objectCount; i++) {
			Vector2 randomPos = randomPosition();
			GameObject tileChoice = objectArray[Random.Range (0, objectArray.Length)];
			placeObjectAt (tileChoice, randomPos);
		}
	}
	void placeObjectAtRandom(GameObject obj){
		placeObjectAtRandom (new GameObject[] {obj}, 1, 1);
	}

	void placePlayer(){
		Vector2 pos = randomPosition ();
		spawnPos = new Vector2[spawnTilesDimension*spawnTilesDimension];
		while(!positionIsInsideMap((int)pos.x+spawnTilesDimension, (int)pos.y+spawnTilesDimension))pos = randomPosition ();
		placeObjectAt (player, pos);
		int count = 0;
		for (int i = 0; i < spawnTilesDimension; i++) {
			for (int j = 0; j < spawnTilesDimension; j++) {
				spawnPos [count] = new Vector2 (pos.x + i, pos.y + j);
				count++;
				placeObjectAt (spawnTile, new Vector3(pos.x+i, pos.y+j, 0.5f));	
			}
		}
		//placeObjectAt (spawnTile, new Vector3(spawnPos.x, spawnPos.y, -0.5f));
	}

	public void placeBuilding(GameObject selectedBuilding, Vector2 mousePosition){
		if (selectedBuilding != null) {
			placeObjectAt (selectedBuilding, mousePosition);
			Debug.Log ("batiment crée");
		}
	}
	public void spawnEnemy(GameObject enemy){
		GameObject instance=Instantiate (enemy, randomSpawnPosition(), enemy.transform.rotation); //obj.transform.rotation = default gameObjetct rotation rotation
		instance.transform.SetParent (boardHolder);
	}
	//delete GameObject from the slotgrid
	public void clearPosition(Vector3 pos){
		int x = (int)Mathf.Round(pos.x);
		int y = (int)Mathf.Round(pos.y);
		slotsGrid [x, y] = null;
		updatePathValue (x, y);
	}
	//delete and destroy GameObject from the slotgrid
	public void resetPosition(Vector3 pos){
		int x = (int)Mathf.Round(pos.x);
		int y = (int)Mathf.Round(pos.y);
		Destroy(slotsGrid [x, y]);
		clearPosition (pos);
	}
	public int getColumns(){
		return columns;
	}
	public int getRows(){
		return rows;
	}
	public void mapConfiguration(int level){
		switch (level) {
		case 1:
			columns = 10;
			rows = 10;
			copperVeinMaxCount = 4;
			copperVeinMinCount = 3;
			ironVeinMaxCount = 3;
			ironVeinMinCount = 2;
			coalVeinMaxCount = 4;
			coalVeinMinCount = 3;
			uraniumVeinMaxCount = 2;
			uraniumVeinMinCount = 1;
			spawnTilesDimension = 2;
			break;
		case 2:
			columns = 50;
			rows = 50;
			copperVeinMaxCount = 100;
			copperVeinMinCount = 75;
			ironVeinMaxCount = 75;
			ironVeinMinCount = 50;
			coalVeinMaxCount = 100;
			coalVeinMinCount = 75;
			uraniumVeinMaxCount = 50;
			uraniumVeinMinCount = 25;
			spawnTilesDimension = 5;
			break;
		case 3:
			columns = 100;
			rows = 100;
			copperVeinMaxCount = 400;
			copperVeinMinCount = 300;
			ironVeinMaxCount = 300;
			ironVeinMinCount = 200;
			coalVeinMaxCount = 400;
			coalVeinMinCount = 300;
			uraniumVeinMaxCount = 200;
			uraniumVeinMinCount = 100;
			spawnTilesDimension = 8;
			break;
		}
		wallMaxCount=(columns*rows)-copperVeinMaxCount-ironVeinMaxCount-coalVeinMaxCount-uraniumVeinMaxCount - spawnTilesDimension*spawnTilesDimension;
		wallMinCount=(int)wallMaxCount-(columns+rows)/2*4;

	}
	public void setupMap(int level){
		mapConfiguration (level);
		initializeMap ();
		placePlayer ();
		placeObjectAtRandom (ironVeinTiles, ironVeinMinCount, ironVeinMaxCount);
		placeObjectAtRandom (coalVeinTiles, coalVeinMinCount, coalVeinMaxCount);
		placeObjectAtRandom (copperVeinTiles, copperVeinMinCount, copperVeinMaxCount);
		placeObjectAtRandom (uraniumVeinTiles, uraniumVeinMinCount, uraniumVeinMaxCount);
		placeObjectAtRandom (wallTiles, wallMinCount, wallMaxCount);
	}
}
