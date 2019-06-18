using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour {

	public static  bool IsPaused = false;
	public GameObject MenuPausaUI, panelEstrellas, Panel;
	public SceneChanger sceneChanger;

	void Update(){
		bool bandera = ClickMouse.IsGalery;
        bool bandera2 = ShowMochila.IsBackPack;
		if (!bandera || !bandera2){
			if (Input.GetKeyDown(KeyCode.Return)){
				if (IsPaused){
					Continuar();
				} else {
					Pause();
				}
			}
  
		}
	}

	public void Continuar(){
		Time.timeScale = 1f;
		IsPaused = false;
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
		MenuPausaUI.SetActive(false);
		Panel.SetActive(true);
		GameManager.instance.paused = false;
	}

	void Pause(){
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		MenuPausaUI.SetActive(true);
		Panel.SetActive(false);
		Time.timeScale = 0f;
		IsPaused = true;
		GameManager.instance.paused = true;
	}

	public void MenuMapa(){
		IsPaused = false;
		Time.timeScale = 1f;
		GameObject.Find("Audio").GetComponent<SoundManager>().PauseAudio();
		sceneChanger.FadeToLevel(GameManager.instance.currentStation);
	}
}
