using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ChallengeVerification : MonoBehaviour
{
    public GameObject panelDesafio, panelPersonaje;
    public Text dialogoPersonaje;
    private string texto;
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

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            if (Pick.recogidos == 3)
            {
                StartCoroutine(Complete());
                
            }
            else
            {
                StartCoroutine(Fail());
            }
            
        }
    }

    IEnumerator Complete()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        panelDesafio.SetActive(true);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        texto = "Buen trabajo gracias a ti hemos podido salvar al Gavilan Dorsigris, continua hacia la siguiente estacion";
        StartCoroutine(Dialogo(panelDesafio, dialogoPersonaje, texto));
        yield return new WaitForSeconds(10.0f);
        Continuar();
        DestroyScriptInstance();

    }

    IEnumerator Fail()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        panelDesafio.SetActive(true);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        texto = "No has conseguido la cantidad de alimento suficiente para ayudar al Gavilan Dorsigris, " +
            "busca ratones o salamandras para ayudarlo";
        StartCoroutine(Dialogo(panelDesafio, dialogoPersonaje, texto));
        yield return new WaitForSeconds(10.0f);
        Continuar();

    }

    IEnumerator Dialogo(GameObject canvasDialogo, Text dialogoPersonaje, string texto)
    {
        int longitud = texto.Length;
        int contador = 0;
        canvasDialogo.SetActive(true);
        dialogoPersonaje.text = string.Empty;
        while (contador < longitud)
        {
            dialogoPersonaje.text = dialogoPersonaje.text + texto[contador];
            contador += 1;
            yield return new WaitForSeconds(0.003f);
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        panelDesafio.SetActive(false);
        panelPersonaje.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
    }

    void DestroyScriptInstance()
    {
        // Removes this script instance from the game object
        Destroy(this);
    }

    
}
