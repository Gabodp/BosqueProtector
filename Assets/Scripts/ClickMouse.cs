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

	public string titulo;
	public string cuerpo;
	public Text txttitulo;
	public Text txtcuerpo;


	void Start () {
		Panel.SetActive(false);		
	}

    void OnMouseDown ()
    {
    	if (MenuPausa.IsPaused == false) {
	    	txttitulo.GetComponent<Text>().text = titulo;
	    	txtcuerpo.GetComponent<Text>().text = cuerpo;
	        Panel.SetActive(true);
	        Galeria.SetActive(true);
	        Panel3.SetActive(false);
	        IsGalery = true;
	        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
			Time.timeScale = 0f;
    	}
    }

	void Update()
    {
		if (Input.GetKeyUp(KeyCode.Q)) {
			Continuar();
        }
        
    }
    
    public void Pause(){
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0f;
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
	}

	public bool GetIsGalery() {
		return IsGalery;
	}
    
}
