using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ShowMochila : MonoBehaviour
{
    public Canvas mochila;
    public static bool IsBackPack = false;

    public void ShowBackPack()
    {
        if (MenuPausa.IsPaused == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mochila.enabled = true;
            IsBackPack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
            GameManager.instance.paused = true;
            Time.timeScale = 0f;
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        mochila.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        IsBackPack = false;
        GameManager.instance.paused = false;
    }

    private void Start()
    {
        mochila.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            if(IsBackPack)
            {
                Continuar();
            }else
            {
                ShowBackPack();
            }
        }

    }
}
