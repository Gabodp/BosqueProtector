using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WallTrigger_2 : MonoBehaviour
{

    public Estacion station;
    public GameObject stationScreen, panelPersonaje, panelEstrellas, canvasDialogo, canvasRespuestas, Panel, mira;
    public CharacterQuestions personaje;
    public Text stationText, dialogoPersonaje, cantidadEstrellas, pregunta;
    private string texto;
    public Button m_opcionA, m_opcionB, m_opcionC, m_opcionD;
    private int value_A, value_B, value_C, value_D;
    private string respuesta;
    private string feedback;
    public string WEB_URL = "";
    public string id;
    public string[] ids_species;
    private List<PreguntaObject> questions;
    static List<string> usadas;

    void Start()
    {
        usadas = new List<string>();
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        GameObject.FindGameObjectWithTag("Mira").GetComponent<MouseController>().enabled = false;
        Panel.SetActive(false);
        mira.SetActive(false);
        panelEstrellas.SetActive(true);
        panelPersonaje.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        m_opcionA.onClick.AddListener(delegate { Wrapper(value_A); });
        m_opcionB.onClick.AddListener(delegate { Wrapper(value_B); });
        m_opcionC.onClick.AddListener(delegate { Wrapper(value_C); });
        m_opcionD.onClick.AddListener(delegate { Wrapper(value_D); });
        texto = "It's question time!! Ahora comprobaremos lo que has aprendido. Aquí va la pregunta..";
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        yield return new WaitForSeconds(10.0f);
        canvasDialogo.SetActive(false);
        canvasRespuestas.SetActive(true);
    }

    public void Continuar()
    {
        Time.timeScale = 1f;
        personaje.PersonajeRestart();
        panelPersonaje.SetActive(false);
        panelEstrellas.SetActive(false);
        canvasDialogo.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        GameObject.FindGameObjectWithTag("Mira").GetComponent<MouseController>().enabled = true;
        Panel.SetActive(true);
        mira.SetActive(true);
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
        canvasDialogo.SetActive(true);
        personaje.PersonajeTriste();
        Debug.Log("triste");
        texto = "Oops!! Parece que no has prestado atención. No hay estrellas para ti por ahora.\n \n \n" + "Respuesta correcta: " + respuesta + "\n \n" + feedback;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        yield return new WaitForSeconds(25.0f);
        Continuar();
    }

    IEnumerator RespuestaCorrecta()
    {
        canvasRespuestas.SetActive(false);
        canvasDialogo.SetActive(true);
        personaje.PersonajeFeliz();
        texto = "Felicidades!! Has acertado con tu respuesta. Te has ganado 5 estrellas. Ahora continúa aprendiendo\n \n \n" + "Respuesta correcta: " + respuesta + "\n \n" + feedback;
        StartCoroutine(Dialogo(canvasDialogo, dialogoPersonaje, texto));
        int x = 0;
        int.TryParse(cantidadEstrellas.text, out x);
        x += 5;
        cantidadEstrellas.text = x.ToString();
        yield return new WaitForSeconds(25.0f);
        Continuar();
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

    void GetPreguntaObjects(PreguntaObjectList objectList)
    {
        questions = new List<PreguntaObject>();
        foreach (PreguntaObject root in objectList.preguntas)
        {
            //Debug.Log(id);
            //Debug.Log(root.QuestionId);
            if (isInEstacion(root.QuestionId))
            {
                if (questions == null)
                {
                    Debug.Log("es null");
                    questions.Add(root);
                }
                questions.Add(root);
            }

            /*if (id.Equals(root.QuestionId))
            {
                pregunta.text = root.Text;
                m_opcionA.GetComponentInChildren<Text>().text = "A. " + root.Options[0];
                m_opcionB.GetComponentInChildren<Text>().text = "B. " + root.Options[1];
                m_opcionC.GetComponentInChildren<Text>().text = "C. " + root.Options[2];
                m_opcionD.GetComponentInChildren<Text>().text = "D. " + root.Options[3];
                value_A = 0;
                value_B = 0;
                value_C = 0;
                value_D = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (root.Answer == 0)
                    {
                        value_A = 1;
                    }
                    else if (root.Answer == 1)
                    {
                        value_B = 1;
                    }
                    else if (root.Answer == 2)
                    {
                        value_C = 1;
                    }
                    else
                    {
                        value_D = 1;
                    }
                }

                respuesta = root.Options[root.Answer];
                feedback = root.Feedback;
            }*/

        }

        bool repeat = true;
        while (repeat)
        {
            int rdn = Random.Range(0, questions.Count);
            Debug.Log(rdn);
            Debug.Log(questions.Count);
            foreach (string t in usadas)
            {
                Debug.Log(t);
            }
            PreguntaObject q = questions[rdn];
            if (!usadas.Contains(q.QuestionId))
            {
                usadas.Add(q.QuestionId);
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

    bool isInEstacion(string pregunta_id)
    {
        for (int i = 0; i < ids_species.Length; i++)
        {
            if (ids_species[i].Equals(pregunta_id))
            {
                return true;
            }
        }
        return false;
    }
}