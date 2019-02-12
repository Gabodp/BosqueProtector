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
    private string[] ids_preguntas;
    private List<PreguntaObject> questions;
    static List<string> usadas;
    private bool begin;
    private PreguntaObject q;
    public RawImage imagen;
    private SpecieObject tmp = null;
    public int n_estacion;

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
            GetSpecies();
            leerInfoPreguntas();
            StartCoroutine(RestClient.Instance.Get(WEB_URL, GetPreguntaObjects));
            StartCoroutine(Preguntas());
            
        }
    }

    IEnumerator Preguntas()
    {
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
        Time.timeScale = 1f;
        personaje.PersonajeRestart();
        panelPersonaje.SetActive(false);
        //panelEstrellas.SetActive(false);
        estrellas.SetActive(false);
        canvasDialogo.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>().enabled = true;
        Panel.SetActive(true);
        mira.SetActive(true);
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
        canvasRespuestas.SetActive(false);
        panelEstrellas.SetActive(true);
        canvasDialogo.SetActive(true);
        personaje.PersonajeTriste();
        Debug.Log("triste");
        texto = "\n \n \n \n" + "Respuesta correcta: " + respuesta + "\n \n" + feedback;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        yield return new WaitForSeconds(25.0f);
        Continuar();
    }

    IEnumerator RespuestaCorrecta()
    {
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
        yield return new WaitForSeconds(25.0f);
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
            if (isInEstacion(root.Id))
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
                /*foreach(SpecieObject specie in especies)
                {
                    if (q.SpecieId == specie.Id)
                    {
                        StartCoroutine(LoadImage(specie.Gallery[0].Id, specie.Id));
                    }
                    
                }*/
                int sid = leerInfoSpecie(q.Id);
                StartCoroutine(CargarInfo(sid));
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

    bool isInEstacion(int pregunta_id)
    {
        for (int i = 0; i < ids_preguntas.Length; i++)
        {
            if (System.Convert.ToInt32(ids_preguntas[i]) == pregunta_id)
            {
                return true;
            }
        }
        return false;
    }


    public IEnumerator CargarInfo(int sid)
    {

        //UnityWebRequest www = UnityWebRequest.Get("200.126.14.250/api/bpv/specie?name=" + "Iguana");
        UnityWebRequest www = UnityWebRequest.Get("200.126.14.250/api/bpv/specie/" + sid);
        //Debug.Log("URL de la info:" + "200.126.14.250/api/bpv/specie?name=" + "Ardilla");
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
                Debug.Log("No hay datos de este árbol");
            }
        }

        StartCoroutine(LoadImage(tmp.Gallery[0].Id, tmp.Id));
        
    }

    public IEnumerator LoadImage(int id, int idSpecie)
    {
        WWW wwwLoader = new WWW("200.126.14.250/api/bpv/specie/" + idSpecie + "/gallery/" + id + "/");
        Debug.Log("URL de la imagen: " + "200.126.14.250/api/bpv/specie/" + idSpecie + "/gallery/" + id + "/");
        yield return wwwLoader;

        imagen.texture = wwwLoader.texture;
    }

    void leerInfoPreguntas()
    {
        string path = "Assets/Resources/infoPreguntas.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string linea = reader.ReadLine();
        while(linea != null)
        {
            //Debug.Log("while");
            string[] var = linea.Split('|');
            int z = 0;
            int.TryParse(var[0], out z);
            //Debug.Log("var z:" + z);
            if (n_estacion == z)
            {
                ids_preguntas = var[1].Split(',');
            }
            linea = reader.ReadLine();
        }
        reader.Close();
    }

    int leerInfoSpecie(int ssid)
    {
        int num = 0;
        string path = "Assets/Resources/infoSpecie.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string linea = reader.ReadLine();
        while (linea != null)
        {
            //Debug.Log("while");
            string[] var = linea.Split('|');
            int w = 0;
            int.TryParse(var[0], out w);
            //Debug.Log("var z:" + w);
            if (ssid == w)
            {
                //ids_preguntas = var[1].Split(',');
                num = System.Convert.ToInt32(var[1]);
                return num;
            }
            linea = reader.ReadLine();
        }
        reader.Close();
        return num;
    }

    public void GetSpecies() {
        foreach(Transform child in station.transform) {
            if (child.tag == "Especies") {
                GameObject especies = child.gameObject;
                foreach (Transform specie in especies.transform) {
                    Debug.Log(specie);
                }
            }
        }
    }
}