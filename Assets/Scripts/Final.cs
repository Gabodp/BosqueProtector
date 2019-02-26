using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Final : MonoBehaviour {

    public Estacion station;
    public GameObject stationScreen, panelPersonaje, canvasDialogo, Panel, mira;
    public Text stationText, dialogoPersonaje, cantidadEstrellas, cantidadDesafios, cantidadEstaciones;
    private string texto, nestrellas, ndesafios, nestaciones;
    private string final;
    private bool begin;

    void Start()
    {
        begin = false;
    }

    void Update()
    {
        if (MenuPausa.IsPaused)
        {
            begin = true;
        }
        else
        {
            if (!MenuPausa.IsPaused && begin)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
            }
            begin = false;

        }
    }

    void DestroyScriptInstance()
    {
        // Removes this script instance from the game object
        Destroy(this);
    }


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
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        Panel.SetActive(false);
        mira.SetActive(false);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        int stars = 0;
        int challenges = 0;
        int stations = 0;
        int.TryParse(cantidadEstrellas.text, out stars);
        int.TryParse(cantidadDesafios.text, out challenges);
        int.TryParse(cantidadEstaciones.text, out stations);

        texto = "Felicidades has completado el recorrido con exito !!!";
        nestrellas = "\n \n - Conseguiste " + stars.ToString() + " estrellas";
        ndesafios = "\n \n - Desafios completados correctamente " + challenges.ToString() + " de 6";
        nestaciones = "\n \n - Estaciones visitadas " + stations.ToString() + " de 6";
        final = texto + nestrellas + ndesafios + nestaciones;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, final));
        yield return new WaitForSeconds(20.0f);
        canvasDialogo.SetActive(false);
        Continuar();
        
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        panelPersonaje.SetActive(false);
        canvasDialogo.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
        mira.SetActive(true);
        Panel.SetActive(true);
        DestroyScriptInstance();
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
