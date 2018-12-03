using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WallTrigger : MonoBehaviour
{

    public Estacion station;
    public GameObject stationScreen;
    public Text stationText;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            if (GameManager.instance.currentStation != station.ID)
            {
                GameManager.instance.currentStation = station.ID;
                stationScreen.SetActive(true);
                if (MapManager.diccionarioNombre.ContainsKey(station.ID))
                {
                    stationText.text = MapManager.diccionarioNombre[station.ID];
                    StartCoroutine(LateCall());
                    Debug.Log(MapManager.diccionarioID[station.ID]);
                    GameObject.Find("Audio").GetComponent<SoundManager>().PlayAudio(MapManager.diccionarioID[station.ID]);
                }
                else
                {
                    stationText.text = "Estación sin nombre";
                    StartCoroutine(LateCall());
                }
            }
        }
    }

    public IEnumerator LateCall()
    {
        yield return new WaitForSeconds(2);
        stationScreen.SetActive(false);
    }
}