using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WallTrigger_2 : MonoBehaviour {

	public Estacion station;
	public GameObject stationScreen, panelPersonaje, panelEstrellas, canvasDialogo, canvasRespuestas, Panel;
	public CharacterQuestions personaje;
	public Text stationText, dialogoPersonaje, cantidadEstrellas;
	public string texto;
	public Button m_opcionA, m_opcionB, m_opcionC, m_opcionD;
    public int value_A, value_B, value_C, value_D;

	void OnTriggerEnter(Collider obj){
		if (obj.gameObject.tag == "Player"){
			StartCoroutine(Preguntas());
		}
	}

	IEnumerator Preguntas() {
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		Panel.SetActive(false);
		panelEstrellas.SetActive(true);
		panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 1f;
		m_opcionA.onClick.AddListener(delegate {Wrapper(value_A);});
		m_opcionB.onClick.AddListener(delegate {Wrapper(value_B);});
		m_opcionC.onClick.AddListener(delegate {Wrapper(value_C);});
		m_opcionD.onClick.AddListener(delegate {Wrapper(value_D);});
		texto = "It's question time!! Ahora comprobaremos lo que has aprendido. Aquí va la pregunta..";
		StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
		yield return new WaitForSeconds(10.0f);
		canvasDialogo.SetActive(false);
		canvasRespuestas.SetActive(true);
	}

	public void prueba(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Debug.Log("prueba");
	}

	public void Continuar(){
		Time.timeScale = 1f;
        personaje.PersonajeRestart();
		panelPersonaje.SetActive(false);
		panelEstrellas.SetActive(false);
		canvasDialogo.SetActive(false);
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
		Panel.SetActive(true);
	}

	public void Wrapper(int i) {
		if(i == 1) {
			StartCoroutine(RespuestaCorrecta());
		} else {
			StartCoroutine(RespuestaIncorrecta());
		}
	}

	IEnumerator RespuestaIncorrecta() {
		canvasRespuestas.SetActive(false);
		canvasDialogo.SetActive(true);
		personaje.PersonajeTriste();
		Debug.Log("triste");
		texto = "Oops!! Parece que no has prestado atención. No hay estrellas para ti por ahora.";
		StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
		yield return new WaitForSeconds(10.0f);
        Continuar();
	}

	IEnumerator RespuestaCorrecta() {
		canvasRespuestas.SetActive(false);
		canvasDialogo.SetActive(true);
		personaje.PersonajeFeliz();
		texto = "Felicidades!! Has acertado con tu respuesta. Te has ganado 5 estrellas. Ahora continúa aprendiendo";
		StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
		int x = 0;
		int.TryParse(cantidadEstrellas.text, out x);
		x += 5;
		cantidadEstrellas.text = x.ToString();
		yield return new WaitForSeconds(10.0f);
        Continuar();
	}

	IEnumerator Dialogo(GameObject canvasDialogo, Text dialogoPersonaje, string texto) {
		int longitud = texto.Length;
		int contador = 0;
		canvasDialogo.SetActive(true);
		dialogoPersonaje.text = "";
		while(contador < longitud) {
			dialogoPersonaje.text = dialogoPersonaje.text + texto[contador];
			contador += 1;
			yield return new WaitForSeconds(0.001f);
		}
	}

	
}