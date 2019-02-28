using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;

public class WallTrigger_2 : MonoBehaviour
{

    public Estacion station;
    public GameObject stationScreen, panelPersonaje, panelEstrellas, canvasDialogo, canvasRespuestas, Panel, mira, estrellas;
    public CharacterQuestions personaje;
    public Text stationText, dialogoPersonaje, cantidadEstrellas, pregunta, desafio;
    private string texto;
    public Button m_opcionA, m_opcionB, m_opcionC, m_opcionD;
    private int value_A, value_B, value_C, value_D;
    private string respuesta;
    private string feedback;
    public string WEB_URL = "";
    private string API = "http://200.126.14.250/api/bpv/specie/";
    private List<PreguntaObject> questions;
    static List<string> usadas;
    private bool begin;
    private PreguntaObject q;
    public RawImage imagen;
    private SpecieObject tmp = null;
    public int n_estacion;
    public Text skip;

    void Start()
    {
        usadas = new List<string>();
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

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Continuar();
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
            StartCoroutine(RestClient.Instance.Get(WEB_URL, GetPreguntaObjects));
            StartCoroutine(Preguntas());   
        }
    }

    IEnumerator Preguntas()
    {
        skip.enabled = false;
        int xx = 0;
        int.TryParse(stationText.text, out xx);
        xx += 1;
        stationText.text = xx.ToString();
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = false;
        Panel.SetActive(false);
        mira.SetActive(false);
        estrellas.SetActive(false);
        imagen.enabled = false;
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        m_opcionA.onClick.AddListener(delegate { Wrapper(value_A); });
        m_opcionB.onClick.AddListener(delegate { Wrapper(value_B); });
        m_opcionC.onClick.AddListener(delegate { Wrapper(value_C); });
        m_opcionD.onClick.AddListener(delegate { Wrapper(value_D); });
        texto = "Hola, veamos si has prestado atencion, a ver si puedes con el siguiente desafio";
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        yield return new WaitForSeconds(10.0f);
        canvasDialogo.SetActive(false);
        canvasRespuestas.SetActive(true);
    }

    public void Continuar()
    {
        begin = false;
        skip.enabled = false;
        Time.timeScale = 1f;
        personaje.PersonajeRestart();
        panelPersonaje.SetActive(false);
        estrellas.SetActive(false);
        canvasDialogo.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
        Panel.SetActive(true);
        mira.SetActive(true);
        imagen.enabled = false;
        DestroyScriptInstance();
    }

    public void Wrapper(int i)
    {
        if (i == 1)
        {
            StartCoroutine(RespuestaCorrecta());
        }
        else
        {
            StartCoroutine(RespuestaIncorrecta());
        }
    }

    IEnumerator RespuestaIncorrecta()
    {
        skip.enabled = true;
        canvasRespuestas.SetActive(false);
        panelEstrellas.SetActive(true);
        imagen.enabled = true;
        canvasDialogo.SetActive(true);
        personaje.PersonajeTriste();
        Debug.Log("triste");
        texto = "\n \n \n \n" + "Respuesta correcta: " + respuesta + "\n \n" + feedback;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        yield return new WaitForSeconds(20.0f);
        Continuar();
    }

    IEnumerator RespuestaCorrecta()
    {
        skip.enabled = true;
        canvasRespuestas.SetActive(false);
        panelEstrellas.SetActive(true);
        estrellas.SetActive(true);
        imagen.enabled = true;
        canvasDialogo.SetActive(true);
        personaje.PersonajeFeliz();
        texto = "\n \n \n \n" + "Respuesta correcta: " + respuesta + "\n \n" + feedback;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        int x = 0;
        int y = 0;
        int.TryParse(cantidadEstrellas.text, out x);
        int.TryParse(desafio.text, out y);
        x += 5;
        y += 1;
        cantidadEstrellas.text = x.ToString();
        desafio.text = y.ToString();
        yield return new WaitForSeconds(20.0f);
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
            yield return new WaitForSeconds(0.001f);
        }
    }

    void GetPreguntaObjects(PreguntaObjectList objectList)
    {
        questions = new List<PreguntaObject>();
        foreach (PreguntaObject root in objectList.preguntas)
        {
            if (isInEstacion(root.Stations))
            {
                if (questions == null)
                {
                    Debug.Log("es null");
                    questions.Add(root);
                }
                questions.Add(root);
            }

        }

        bool repeat = true;
        while (repeat)
        {
            Debug.Log("COUNT:" + questions.Count);
            int rdn = Random.Range(0, questions.Count);
            Debug.Log(rdn);
            Debug.Log(questions.Count);
            foreach (string t in usadas)
            {
                Debug.Log(t);
            }

            q = questions[rdn];
            if (!usadas.Contains(q.QuestionId))
            {
                usadas.Add(q.QuestionId);
                StartCoroutine(CargarInfo(q.SpecieId));
                pregunta.text = q.Text;
                m_opcionA.GetComponentInChildren<Text>().text = "A. " + q.Options[0];
                m_opcionB.GetComponentInChildren<Text>().text = "B. " + q.Options[1];
                m_opcionC.GetComponentInChildren<Text>().text = "C. " + q.Options[2];
                m_opcionD.GetComponentInChildren<Text>().text = "D. " + q.Options[3];
                value_A = 0;
                value_B = 0;
                value_C = 0;
                value_D = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (q.Answer == 0)
                    {
                        value_A = 1;
                    }
                    else if (q.Answer == 1)
                    {
                        value_B = 1;
                    }
                    else if (q.Answer == 2)
                    {
                        value_C = 1;
                    }
                    else
                    {
                        value_D = 1;
                    }
                }

                respuesta = q.Options[q.Answer];
                feedback = q.Feedback;
                repeat = false;
            }
            else
            {
                repeat = true;
            }
        }

    }

    bool isInEstacion(List<StationP> estaciones)
    {
        foreach(StationP sp in estaciones)
        {
            if(sp.GameStation == n_estacion)
            {
                return true;
            }
        }
        return false;
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
                tmp  = JsonUtility.FromJson<SpecieObject>(data);
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
        WWW wwwLoader = new WWW(API + idSpecie + "/gallery/" + id + "/");
        yield return wwwLoader;

        imagen.texture = wwwLoader.texture;
    }

    public void GetSpecies() {
        foreach(Transform child in station.transform) {
            if (child.tag == "Especies") {
                GameObject especies = child.gameObject;
                int size = especies.transform.childCount;
                string[] names = new string[size];
                int i = 0;
                foreach (Transform specie in especies.transform) {
                    names[i] = specie.GetComponent<ClickMouse>().specieName;
                    i++;
                }
                
                for (int j = 0; j < names.Length; j++)
                {
                    Debug.Log("names[" + j + "] = " + names[j]);
                }
            }
        }
    }
}