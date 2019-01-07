using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ClickMouse : MonoBehaviour {

	public GameObject Panel;
	public GameObject Galeria;
	public GameObject Panel3;
	public static bool IsGalery = false;
	public string name;

	void Start () {
		Panel.SetActive(false);		
	}

    void OnMouseDown () {
    	if (MenuPausa.IsPaused == false) {
	        Panel.SetActive(true);
	        Galeria.SetActive(true);
	        Panel3.SetActive(false);
			Galeria.GetComponent<Galery>().name = name;
	        IsGalery = true;
	        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
			GameManager.instance.paused = true;
			Time.timeScale = 0f;
    	}
    }

	void Update()
    {
		if (Input.GetKeyUp(KeyCode.Q))
        {
			Continuar();
        }
        
    }
    
    public void Pause(){
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;
		GameManager.instance.paused = true;
	}
	
	public void Continuar(){
		Time.timeScale = 1f;
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Panel.SetActive(false);
		Panel3.SetActive(true);
		Galeria.SetActive(false);
		IsGalery = false;
		GameManager.instance.paused = false;
	}

	public bool GetIsGalery() {
		return IsGalery;
	}
    
}
