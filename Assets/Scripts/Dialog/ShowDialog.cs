using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ShowDialog : MonoBehaviour
{
    public Canvas dialog;
    public string[] texto;
    public Button step;
    public Text textDialog;
    private int n_dialogues;
    public bool disappear;

    private void Start()
    {
        dialog.enabled = false;
        n_dialogues = 0;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            IntroductionDialog();
        }
    }

    public void IntroductionDialog()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        dialog.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        step.onClick.AddListener(delegate { ShowingDialog();});
        if(texto.Length > 0)
        {
            step.enabled = false;
            StartCoroutine(Dialogo(dialog, textDialog, texto[n_dialogues]));
            n_dialogues++;
        }else
        {
            Debug.Log("No hay dialogos");
        }

    }

    public void IntroductionDialog(int value)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        dialog.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        step.onClick.AddListener(delegate { Continuar(); });
        step.enabled = false;
        StartCoroutine(Dialogo(dialog, textDialog, texto[value]));
    }

    private void ShowingDialog()
    {
        if(n_dialogues < texto.Length)
        {
            if(step.enabled)
            {
                step.enabled = false;
                StartCoroutine(Dialogo(dialog, textDialog, texto[n_dialogues]));
                n_dialogues++;
            }
            
        }
        else
        {
            Continuar();
        }
    }

    IEnumerator Dialogo(Canvas canvasDialogo, Text dialogoPersonaje, string texto)
    {
        int longitud = texto.Length;
        int contador = 0;
        
        dialogoPersonaje.text = string.Empty;
        while (contador < longitud)
        {
            dialogoPersonaje.text = dialogoPersonaje.text + texto[contador];
            contador += 1;
            yield return new WaitForSeconds(0.001f);
        }
        step.enabled = true;
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
        dialog.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(disappear == true)
        {
            DestroyScriptInstance();
        }else
        {
            n_dialogues = 0;
        }
        
    }

    void DestroyScriptInstance()
    {
        Destroy(this);
    }
}
