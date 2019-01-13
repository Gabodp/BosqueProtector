using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Final : MonoBehaviour {

    public Estacion station;
    public GameObject stationScreen, panelPersonaje, canvasDialogo, Panel;
    public Text stationText, dialogoPersonaje, cantidadEstrellas;
    private string texto;
    private string texto2;
    private string final;
 

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            StartCoroutine(Preguntas());
        }
    }

    IEnumerator Preguntas()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Mira").GetComponent<MouseController>().enabled = false;
        Panel.SetActive(false);
        //panelEstrellas.SetActive(true);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        int stars = 0;
        int.TryParse(cantidadEstrellas.text, out stars);
        texto = "Felicidades has completado el recorrido con exito !!!";
        texto2 = "\n \n Conseguiste " + stars.ToString() + " estrellas";
        final = texto + texto2;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, final));
        yield return new WaitForSeconds(10.0f);
        canvasDialogo.SetActive(false);
        Continuar();
        
    }

    public void prueba()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("prueba");
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        panelPersonaje.SetActive(false);
        //panelEstrellas.SetActive(false);
        canvasDialogo.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Mira").GetComponent<MouseController>().enabled = true;
        Panel.SetActive(true);
    }

    IEnumerator Dialogo(GameObject canvasDialogo, Text dialogoPersonaje, string texto)
    {
        int longitud = texto.Length;
        int contador = 0;
        canvasDialogo.SetActive(true);
        dialogoPersonaje.text = "";
        while (contador < longitud)
        {
            dialogoPersonaje.text = dialogoPersonaje.text + texto[contador];
            contador += 1;
            yield return new WaitForSeconds(0.001f);
        }
    }

    
}
