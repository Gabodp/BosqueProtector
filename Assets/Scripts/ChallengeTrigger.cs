using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChallengeTrigger : MonoBehaviour
{
    public GameObject panelDesafio, panelPersonaje;
    public Text dialogoPersonaje;
    public RawImage imagen;
    private string texto;
    private bool begin;
    private string API = "http://200.126.14.250/api/bpv/specie/";
    private SpecieObject tmp = null;
    private GameObject [] recolectables;

    void Start()
    {
        begin = false;
        recolectables = GameObject.FindGameObjectsWithTag("Pick");
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
            foreach(GameObject g in recolectables)
            {
                g.GetComponent<Pick>().activate = true;
            }
            StartCoroutine(Desafios());
        }
    }

    IEnumerator Desafios()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        panelDesafio.SetActive(true);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        texto = "Hey necesitamos tu ayuda el Gavilan Dorsigris se lastimo una de sus alas y no puede conseguir " +
            "su alimento por si mismo, encuentra tres ratones o salamandras para ayudar al Gavilan Dorsigris y " +
            "poder acceder a las siguiente estacion";
        StartCoroutine(CargarInfo(2));//temporal prototipo
        StartCoroutine(Dialogo(panelDesafio, dialogoPersonaje, texto));
        yield return new WaitForSeconds(15.0f);
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
        //begin = false;
        Time.timeScale = 1f;
        panelDesafio.SetActive(false);
        panelPersonaje.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true; 
        DestroyScriptInstance();
    }

    void DestroyScriptInstance()
    {
        // Removes this script instance from the game object
        Destroy(this);
    }

    public IEnumerator CargarInfo(int sid)
    {

        UnityWebRequest www = UnityWebRequest.Get(API + sid + "/");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            try
            {
                tmp = JsonUtility.FromJson<SpecieObject>(data);
                Debug.Log(tmp.Name);
            }
            catch (System.Exception)
            {
                Debug.Log("No hay datos de esta especie");
            }
        }

        StartCoroutine(LoadImage(tmp.Gallery[0].Id, tmp.Id));

    }

    public IEnumerator LoadImage(int id, int idSpecie)
    {
        UnityWebRequest w = UnityWebRequestTexture.GetTexture(API + idSpecie + "/gallery/" + id + "/");
        yield return w.SendWebRequest();
        if (w.isNetworkError || w.isHttpError)
        {
            Debug.Log(w.error);
        }
        else
        {
            imagen.texture = ((DownloadHandlerTexture)w.downloadHandler).texture;
        }

    }

}
