using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Pick : MonoBehaviour
{
    public bool activate = false;
    public static int recogidos;
    public Canvas backpack;
    public int id;
    public GameObject panel;
    public Text n;

    private void OnMouseDown()
    {
        if (activate)
        {
            StartCoroutine(Esperar());

        }
    }

    IEnumerator Esperar()
    {
        recogidos += 1;
        Debug.Log(recogidos);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        n.text = recogidos.ToString();
        panel.SetActive(true);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().Progress(30);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        yield return new WaitForSeconds(0.8f);
        panel.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
        backpack.GetComponent<Inventory>().TestAddF(id);
        Destroy(this.gameObject);
    }


}
