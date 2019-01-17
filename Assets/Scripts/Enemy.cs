using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	//bool needNewPath=true;
	AudioSource audioSource;
	public AudioClip attackSound;

	private float pathCalculationSpeed = 0.2f;
	private float nextPathCalculation;
	// Use this for initialization
	void Start () {
		mapManager=MapManager.instance;
		player = GameManager.playerInstance.gameObject;
		audioSource = gameObject.GetComponent<AudioSource>();

		attackDamage += (int)(GameManager.instance.getWave ()*0.1f*attackDamage);
		hp += (int)(GameManager.instance.getWave ()*0.1f*hp);
		reward+=(int)(GameManager.instance.getWave ()*0.1f*reward);

		path = new List<PathFind.Point>();
	}
	void setupPathfinding(){
		int playerX = (int) Mathf.Round(gameObject.transform.position.x);
		int playerY = (int) Mathf.Round(gameObject.transform.position.y);
		self=new PathFind.Point(playerX, playerY);

		int x = (int) Mathf.Round(player.gameObject.transform.position.x);
		int y = (int) Mathf.Round(player.gameObject.transform.position.y);
		target = new PathFind.Point(x, y);

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
	}

	// Update is called once per frame
	void Update () {
		time=GameManager.instance.getTime();
		updatePath ();
		move();
		if (self.x == target.x && self.y == target.y) {
			rotateToTarget (player.transform.position);
			attack (player);
		}
	}
	void updatePath(){
		if (path.Count == 0 && nextPathCalculation <= time) {
			setupPathfinding();
			nextPathCalculation = time + pathCalculationSpeed;
		}
	}

	void move(){
		if(path.Count == 0){
			Debug.Log(path.Count);
			return;
		}

		PathFind.Point attackTargetPoint = path [path.Count - 1];
		attackTargetPoint = path.FirstOrDefault(p=>mapManager.getObjectAt(p.x, p.y) != null &&
							 mapManager.getObjectAt(p.x, p.y).tag == "Building"); // get the first building in the path ...
		GameObject attackTarget = null;

		// if any building in the path exists, attack it
		if(attackTargetPoint != null){
			attackTarget = 	mapManager.getObjectAt(attackTargetPoint.x, attackTargetPoint.y);
		}
		// else, focus player
		else{
			attackTarget = player;
		}

		if(Vector2.Distance (transform.position, attackTarget.transform.position) <= maxAttackDistance){ // if in range
			attack(attackTarget);
			return;
		}

		// if there is no building or not in range, travel through the path
		PathFind.Point point = path[0];
		GameObject objectAtPoint = mapManager.getObjectAt(point.x, point.y);
		if(objectAtPoint != null && objectAtPoint.tag == "Building" && 
			Vector2.Distance (transform.position, objectAtPoint.transform.position) <= maxAttackDistance){ // if the next position is a building and in range, immediatly {
			return;
		}
		Vector2 objectPos = new Vector2(point.x, point.y);
		if (Vector2.Distance(transform.position, objectPos) > .0001) { //move to next point
			rotateToTarget (objectPos);
			transform.position = Vector2.MoveTowards(transform.position, objectPos, speed * Time.deltaTime);
			return;
		}
		else{
			path.RemoveAt(0); // delete the first point if the enemy is near
			if (playerHasMoved()) //and if the player has moved, the path is obsolete
				path.Clear();
		}
		
	}
	
	bool playerHasMoved(){
		if(path.Count == 0)
			return false;
		PathFind.Point actualPlayerPos = new PathFind.Point ((int)player.gameObject.transform.position.x,
			(int)player.gameObject.transform.position.y);
		return (path [path.Count - 1] != actualPlayerPos);
	}

	void attack(GameObject obj){
		if(Vector2.Distance (transform.position, obj.transform.position) <= maxAttackDistance){
			rotateToTarget (obj.transform.position);
				
			if ( nextAttack <= time ) {
				Animator anim = GetComponent<Animator> ();
				if (anim != null)
					anim.SetTrigger ("attack");
				obj.gameObject.GetComponent<Destructible> ().damage (attackDamage);
				nextAttack = time + attackSpeed;
				audioSource.PlayOneShot (attackSound);
			}
		}
	}
	public override void damage(int loss){
		hp -= loss;
		if (hp <= 0){
			GameManager.instance.decreaseEnemiesLeft ();
			GameManager.playerInstance.getRessources ()["gold"]+=reward;
			Destroy(gameObject);
		}
	}

	void rotateToTarget(Vector2 targetPos){
		Vector3 targetDir = new Vector3(targetPos.x, targetPos.y, 0) - transform.position;
		float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
	}

}
