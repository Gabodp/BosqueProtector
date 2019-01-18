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
		if (!bandera){
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
		//panelEstrellas.SetActive(false);
		Panel.SetActive(true);
	}

	void Pause(){
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		//panelEstrellas.SetActive(true);
		MenuPausaUI.SetActive(true);
		Panel.SetActive(false);
		Time.timeScale = 0f;
		IsPaused = true;
	}

	public void MenuMapa(){
		IsPaused = false;
		Time.timeScale = 1f;
		GameObject.Find("Audio").GetComponent<SoundManager>().PauseAudio();
		sceneChanger.FadeToLevel(GameManager.instance.currentStation);
	}
}
