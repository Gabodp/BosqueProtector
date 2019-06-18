using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	//Esta clase controlará donde aparecerá el jugador en el bosque
	public static GameManager instance = null;
	public MapManager mapManager;
	public GameObject player;
	public GameObject[] spawnArray;
	public int currentStation;
	public int scene;
	public bool paused;

	void Awake () {
		if (instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != null){
			Destroy(gameObject);
		}

		if (player == null){
			player = GameObject.FindGameObjectWithTag("Player");
		}

		if (spawnArray.Length == 0){
			spawnArray = GameObject.FindGameObjectsWithTag("Spawn");
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		player = GameObject.FindGameObjectWithTag("Player");
		spawnArray = GameObject.FindGameObjectsWithTag("Spawn");
		
		try {
            mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
            
        } catch (System.Exception){
			Debug.Log("MapManager not present in this scene.");
		}
		
	
		for (int i = 0; i < spawnArray.Length; i++){
			if (spawnArray[i].GetComponent<Estacion>().ID == currentStation){
				GameObject spawn = spawnArray[i];
				if (this.scene == 1){
                    player.transform.position = spawn.transform.position;
					//player.transform.rotation = spawn.transform.rotation;
				} else if (this.scene == 0){
                    mapManager.PinInicio = spawn.GetComponent<Pin>();
				}
			}
		}

	}

	public void SetCurrentStation(int station){
		currentStation = station;
	}

}
