using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public event System.Action OnLevelInfoUpdateEvent;

	public int baseTimeLeft=30;

	public static GameManager instance = null;
	public static Player playerInstance;
	public static bool gameInitialized;
	public GameObject[] enemies;
	MapManager mapScript;

	float timeLeft;
	float timeUntilNextWave;
	public int spawnTick=3;
	int wave = 1;
	int wavePerLevel=3;
	//public static int level = 1;
	int enemiesCount;
	int enemiesLeft=0;
	private bool levelFinished=false;

	private float nextSpawnTick;
	private float time;
	private int mapSize;

	void Awake(){
		if (instance == null)instance = this;
		mapScript = GetComponent<MapManager>();
	}
	// Use this for initialization
	void Start () {
		gameInitialized = false;
		wave = PlayerPrefs.GetInt ("actualWave", 1);
		mapSize = MenuScript.selectedMapSize;
		nextWaveCalculation ();
		mapScript.setupMap(mapSize);
		playerInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		gameInitialized = true;
	}
	void Update(){
		if (playerInstance != null && !playerInstance.GetComponent<Player> ().isDead ()) {
			timeCalculation ();
			if (timeLeft > 0)
				Countdown ();
			else
				SpawnDelay ();
			if (OnLevelInfoUpdateEvent != null) {
				OnLevelInfoUpdateEvent ();
			}
		}
	}
	void nextWaveCalculation(){
		switch (mapSize) {
		case 1:
			timeLeft = baseTimeLeft - (wave - 1)*2;
			enemiesCount = 5+wave * 2;
			break;
		case 2:
			timeLeft = baseTimeLeft*1.25f - (wave - 1)*2;
			enemiesCount = 10+wave * 3;
			break;
		case 3:
			timeLeft = baseTimeLeft*1.25f - (wave - 1)*2;
			enemiesCount = 10+wave * 4;
			break;
		}
		if (timeLeft < timeLeft / 2)
			timeLeft = timeLeft / 2;
	}
		
	void Countdown(){
		timeLeft -= Time.deltaTime;
		if (timeLeft<0){
			timeLeft = 0;
		}
	}
	void SpawnDelay(){
		if (nextSpawnTick <= time && enemiesCount > 0 && !levelFinished) {
			int spawnRng = Random.Range (0, enemies.Length);
			enemiesCount -= spawnRng + 1;
			mapScript.spawnEnemy (enemies [spawnRng]);
			increaseEnemiesLeft ();
			//enemyCount--;
			//Debug.Log(enemyCount);
			nextSpawnTick = time + spawnTick;
		} 
		else if (enemiesCount <= 0 && wave%wavePerLevel==0 && enemiesLeft<=0) {
			UIManager.instance.getLevelEndMenu().display ();
			levelFinished = true;
			wave++;
		}
		else if (enemiesCount <= 0 && wave%wavePerLevel!=0 && !levelFinished) {
			wave++;
			nextWaveCalculation ();
		}
	}

	void timeCalculation(){
		time += Time.deltaTime;
	}
	public float getTime(){
		return time;
	}
	public float getTimeLeft(){
		return timeLeft;
	}
	public int getWave(){
		return wave;
	}
	public void decreaseEnemiesLeft(){
		enemiesLeft--;
	}
	public void increaseEnemiesLeft(){
		enemiesLeft++;
	}

	void nextLevelSetup (){
		PlayerPrefs.SetInt ("copperAmount", playerInstance.getRessources()["copper"]);
		PlayerPrefs.SetInt ("ironAmount", playerInstance.getRessources()["iron"]);
		PlayerPrefs.SetInt ("coalAmount", playerInstance.getRessources()["coal"]);
		PlayerPrefs.SetInt ("uraniumAmount", playerInstance.getRessources()["uranium"]);
		PlayerPrefs.SetInt ("actualWave", GameManager.instance.getWave ());
	}
	public void nextLevel(){
		//level++;
		PlayerPrefs.SetInt("goldAmount", playerInstance.getRessources()["gold"]);
		nextLevelSetup ();
		PlayerPrefs.Save ();
		SceneManager.LoadScene("Level1");
	}
	public void backToMenu(){
		PlayerPrefs.SetInt("goldAmount", playerInstance.getRessources()["gold"]);
		if(PlayerPrefs.GetInt ("bestWave", 1)<GameManager.instance.getWave())
			PlayerPrefs.SetInt("bestWave", GameManager.instance.getWave());
		if (!playerInstance.isDead ()) {
			nextLevelSetup ();
			PlayerPrefs.SetInt ("actualMapSize", MenuScript.selectedMapSize);
		} else {
			PlayerPrefs.DeleteKey ("copperAmount");
			PlayerPrefs.DeleteKey ("ironAmount");
			PlayerPrefs.DeleteKey ("coalAmount");
			PlayerPrefs.DeleteKey ("uraniumAmount");
			PlayerPrefs.SetInt ("actualWave", 1);
		}

		PlayerPrefs.Save ();
		SceneManager.LoadScene("Menu");
	}
}