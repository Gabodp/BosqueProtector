using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WallTrigger : MonoBehaviour {

	public Estacion station;
	public GameObject stationScreen, panelPersonaje, panelEstrellas, canvasDialogo, canvasRespuestas, Panel;
	public CharacterQuestions personaje;
	public Text stationText, dialogoPersonaje, cantidadEstrellas;
	public string texto;
	public Button m_opcionA, m_opcionB, m_opcionC, m_opcionD;

	void OnTriggerEnter(Collider obj){
		if (obj.gameObject.tag == "Player"){
			/*if (GameManager.instance.currentStation != station.ID){
				GameManager.instance.currentStation = station.ID;
				//stationScreen.SetActive(true);
				if (MapManager.diccionarioNombre.ContainsKey(station.ID)){
					//stationText.text = MapManager.diccionarioNombre[station.ID];
					//StartCoroutine(LateCall());				
					//GameObject.Find("Audio").GetComponent<SoundManager>().PlayAudio(MapManager.diccionarioID[station.ID]);
				}
				else {
					//stationText.text = "Estación sin nombre";
					//StartCoroutine(LateCall());
				}
			}*/
			StartCoroutine(Preguntas());
		}
	}

	IEnumerator Preguntas() {
		GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		//GameObject.Find("Audio").GetComponent<SoundManager>().PauseAudio();
		Panel.SetActive(false);
		panelEstrellas.SetActive(true);
		panelPersonaje.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 1f;
		/*m_opcionA.onClick.AddListener(delegate {Wrapper(0);});
		m_opcionB.onClick.AddListener(delegate {Wrapper(1);});
		m_opcionC.onClick.AddListener(delegate {Wrapper(0);});
		m_opcionD.onClick.AddListener(delegate {Wrapper(0);});*/
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

	public IEnumerator LateCall(float segundos){
		yield return new WaitForSeconds(segundos);
	}
}