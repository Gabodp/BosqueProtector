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
	private Galery GaleryScript;
	public GameObject Panel3;
	public static bool IsGalery = false;
	public string specieName;

	void Start () {
		Panel.SetActive(false);	
		GaleryScript = Galeria.GetComponent<Galery>();	
	}

    public void ShowGallery () {
    	if (MenuPausa.IsPaused == false) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
	        Panel.SetActive(true);
	        Galeria.SetActive(true);
	        Panel3.SetActive(false);
			GaleryScript.name = specieName;
			GaleryScript.visible = true;
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
		Galeria.GetComponent<Galery>().visible=false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Panel.SetActive(false);
		Panel3.SetActive(true);
		Galeria.SetActive(false);
		IsGalery = false;
		GameManager.instance.paused = false;
	}
    
}
