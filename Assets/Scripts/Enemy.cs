using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Destructible {

	MapManager mapManager;
	GameObject player;
	public float speed = 2f;
	public float maxAttackDistance = 1f;
	public int attackDamage = 1;
	public float attackSpeed = 1.5f;
	public int reward;
	PathFind.Point self;
	PathFind.Point target;
	List<PathFind.Point> path;

	private float nextAttack;
	private float time;
	bool needNewPath=false;
	AudioSource audioSource;
	public AudioClip attackSound;

	private float pathCalculationSpeed = 0.2f;
	private float nextPathCalculation;
	// Use this for initialization
	void Start () {
		mapManager=MapManager.instance;
		player = GameManager.playerInstance;
		audioSource = gameObject.GetComponent<AudioSource>();

		int playerX = (int)gameObject.transform.position.x;
		int playerY = (int)gameObject.transform.position.y;
		self=new PathFind.Point(playerX, playerY);

		int x = (int)player.gameObject.transform.position.x;
		int y = (int)player.gameObject.transform.position.y;
		target = new PathFind.Point(x, y);

		attackDamage += (int)(GameManager.instance.getWave ()*0.1f*attackDamage);
		hp += (int)(GameManager.instance.getWave ()*0.1f*hp);
		reward+=(int)(GameManager.instance.getWave ()*0.1f*reward);
		setupPathfinding ();
		StartCoroutine (move ());
	}
	void setupPathfinding(){
		float[,] pathsValuesGrid = mapManager.getPathsValuesGrid ();
		int columns = mapManager.getColumns();
		int rows = mapManager.getRows();
		// every float in the array represent the cost of passing the tile at that position.
		// use 0.0f for blocking tiles.
		// create a grid
		PathFind.Grid grid = new PathFind.Grid(columns, rows, pathsValuesGrid);

		// get path
		// path will either be a list of Points (x, y), or an empty list if no path is found.
		path = PathFind.Pathfinding.FindPath(grid, self, target);
		/*foreach(PathFind.Point point in path){
			Debug.Log ("x:"+point.x+ " y:"+point.y);
		}*/
	}
	void updatePath(){
		self.x = (int)Mathf.Round(gameObject.transform.position.x);
		self.y = (int)Mathf.Round(gameObject.transform.position.y);
		target.x = (int)Mathf.Round(player.gameObject.transform.position.x);
		target.y = (int)Mathf.Round(player.gameObject.transform.position.y);
		setupPathfinding ();

	}
	// Update is called once per frame
	void Update () {
		time=GameManager.instance.getTime();
		reCalculatePath ();
		if (self.x == target.x && self.y == target.y) {
			rotateToTarget (player.transform.position);
			attack (player);
		}
	}
	IEnumerator move(){
		foreach(PathFind.Point point in path) {
			//Debug.Log ("following next point");
			GameObject objAtPoint=mapManager.getObjectAt (point.x, point.y);
			if (objAtPoint!=null && objAtPoint.tag == "Building") {
				needNewPath = true;
				rotateToTarget (objAtPoint.transform.position);
				attack (objAtPoint);
				break;
			}
			Vector2 itemPos = new Vector2(point.x, point.y);
			while (Vector2.Distance(transform.position, itemPos) > .0001) {
				rotateToTarget (itemPos);
				transform.position = Vector2.MoveTowards(transform.position, itemPos, speed * Time.deltaTime);
				yield return null;
			}

			PathFind.Point actualPlayerPos = new PathFind.Point ((int)player.gameObject.transform.position.x,
				(int)player.gameObject.transform.position.y);
			if (path [path.Count - 1] != actualPlayerPos) { //check if the player have moved
				needNewPath = true;
				break;
			}
		}
		needNewPath = true;
	}
	void attack(GameObject obj){
		if (Vector2.Distance (transform.position, obj.transform.position) <= maxAttackDistance && (nextAttack <= time)) {
			Animator anim = GetComponent<Animator> ();
			if (anim != null)
				anim.SetTrigger ("attack");
			obj.gameObject.GetComponent<Destructible> ().damage (attackDamage);
			nextAttack = time + attackSpeed;
			audioSource.PlayOneShot (attackSound);
		}
	}
	public override void damage(int loss){
		hp -= loss;
		if (hp <= 0){
			GameManager.instance.decreaseEnemiesLeft ();
			GameManager.playerInstance.GetComponent<Player>().getRessources ()["gold"]+=reward;
			Destroy(gameObject);
		}

	}
	void rotateToTarget(Vector2 targetPos){
		Vector3 targetDir = new Vector3(targetPos.x, targetPos.y, 0) - transform.position;
		float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
	}

	void reCalculatePath(){
		if (needNewPath && nextPathCalculation <= time) {
			needNewPath = false;
			StopCoroutine ("move");
			updatePath ();
			StartCoroutine ("move");
			nextPathCalculation = time + pathCalculationSpeed;
		}
	}

}
