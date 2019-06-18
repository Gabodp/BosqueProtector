using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Clic : MonoBehaviour
{
    public bool activate = false;
    public static int clickeados;
    public GameObject panel;
    public Text n;
    private bool done = false;

    private void OnMouseDown()
    {
        if (activate)
        {
            StartCoroutine(Esperar());

        }
    }

    IEnumerator Esperar()
    {
        if(!done && Clic.clickeados < 3)
        {
            clickeados += 1;
            Debug.Log(clickeados);
            /*GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;*/
            n.text = clickeados.ToString();
            panel.SetActive(true);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().Progress(30);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;

            yield return new WaitForSeconds(0.8f);
            panel.SetActive(false);
            done = true;
            /*GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;*/
        }

    }
}
