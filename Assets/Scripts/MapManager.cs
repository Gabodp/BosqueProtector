using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Networking;

[Serializable]
public class Station
{
    public string StationId;
    public int Id;
    public string APIKey;
    public string Name;
    public int GameStation;
    public string Latitude;
    public string Longitude;
    public string AndroidVersion;
    public string ServicesVersion;
}

[Serializable]
public class Sensor
{
    public string DataId;
    public int StationId;
    public int SensorId;
    public int Id;
    public int Timestamp;
    public string Type;
    public string Value;
    public string Units;
    public string Location;
}

[Serializable]
public class Stations
{
    public List<Station> estaciones;
}

public class MapManager : MonoBehaviour {
	public Character character;
	public Pin PinInicio;
	public GameObject PanelEstacion;
	public GameObject PanelSensor;
	public Text Estacion;
	public Text Cargando;
	public GameObject Panel;
	public GameObject Canvas;
	public Text TextSensor;
	public SceneChanger sceneChanger;
	//GameStation es el ID
	public static Dictionary<int, int> diccionarioID = new Dictionary<int, int>();
	public static Dictionary<int, String> diccionarioNombre = new Dictionary<int, String>();
   	bool flagT = false;
   	bool flagH = false;

	[DllImport("__Internal")]
    private static extern string GetTemperature(int station_id);
	
	private void Start(){
		StartCoroutine(GetStations());
		Panel.SetActive(false);
		character.Iniciar(this, PinInicio);
		flagT = false;
		flagH = false;
	}

	public void Update(){
		if (character.IsMoving){
			PanelEstacion.SetActive(true);
			PanelSensor.SetActive(true);
			return;
		}
		CheckForInput();
	}

	private void CheckForInput(){
		if (Input.GetKeyUp(KeyCode.Return) && character.PinActual.estacion.ID != 0){
			Panel.SetActive(true);
			if (diccionarioNombre.ContainsKey(character.PinActual.estacion.ID))
			{
				Cargando.text = string.Format("Entrando a: " + diccionarioNombre[character.PinActual.estacion.ID]);
				Canvas.SetActive(false);
			}
			sceneChanger.FadeToLevel(character.PinActual.estacion.ID);
		} else if (Input.GetKeyUp(KeyCode.UpArrow)){
			character.TrySetDireccion(Direccion.Arriba);
		} else if (Input.GetKeyUp(KeyCode.DownArrow)){
			character.TrySetDireccion(Direccion.Abajo);
		} else if (Input.GetKeyUp(KeyCode.LeftArrow)){
			character.TrySetDireccion(Direccion.Izquierda);
		} else if (Input.GetKeyUp(KeyCode.RightArrow)){
			character.TrySetDireccion(Direccion.Derecha);
		}
	}

	public void UpdateGUI(){
		StartCoroutine(GetTemp());
	}

	IEnumerator GetStations()
    {
		using (UnityWebRequest www = UnityWebRequest.Get("http://200.126.14.250/api/Station"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string descarga = www.downloadHandler.text;
                string JSONToParse = "{\"estaciones\":" + descarga + "}";
                
                Stations datos = JsonUtility.FromJson<Stations>(JSONToParse);

                foreach (var dato in datos.estaciones)
	            {
	                if (!diccionarioID.ContainsKey(dato.GameStation))
	                {
	                    diccionarioID.Add(dato.GameStation, dato.Id);
	                }

	                if (!diccionarioNombre.ContainsKey(dato.GameStation))
	                {
	                    diccionarioNombre.Add(dato.GameStation, dato.Name);
	                }
	            }

            }
        }
	}

	IEnumerator GetTemp()
    {
    	if (diccionarioID.ContainsKey(character.PinActual.estacion.ID))
    	{
    		Sensor datos1 = null;
    		Sensor datos2 = null;

    		using (UnityWebRequest www = UnityWebRequest.Get("http://200.126.14.250/api/station/" + diccionarioID[character.PinActual.estacion.ID] + "/sensor/2/data/lastdata"))
	        {
	            yield return www.SendWebRequest();

	            if (www.isNetworkError || www.isHttpError)
	            {
	                Debug.Log(www.error);
	            }
	            else
	            {
	                string descarga = www.downloadHandler.text;

	                try {
						datos1 = JsonUtility.FromJson<Sensor>(descarga);
					} catch (System.Exception){
						flagT = true;
						Debug.Log("No hay datos de Temperatura en la estacion " + diccionarioNombre[character.PinActual.estacion.ID]);
					}
	            }
	        }

	        using (UnityWebRequest www = UnityWebRequest.Get("http://200.126.14.250/api/station/" + diccionarioID[character.PinActual.estacion.ID] + "/sensor/1/data/lastdata"))
	        {
	            yield return www.SendWebRequest();

	            if (www.isNetworkError || www.isHttpError)
	            {
	                Debug.Log(www.error);
	            }
	            else
	            {
	                string descarga = www.downloadHandler.text;
	                
	                try {
	                	datos2 = JsonUtility.FromJson<Sensor>(descarga);
					} catch (System.Exception){
						flagH = true;
						Debug.Log("No hay datos de Humedad en la estacion " + diccionarioNombre[character.PinActual.estacion.ID]);
					}
	            }
	        }

	        if (flagT & flagH){
	        	TextSensor.text = string.Format("Temperatura:\nHumedad:");
	        } else if (flagT) {
	        	TextSensor.text = string.Format("Humedad: {2} {3}", datos2.Value, datos2.Units);
	        } else if (flagH) {
	        	TextSensor.text = string.Format("Temperatura: {0} {1}", datos1.Value, datos1.Units);
	        } else {
	        	TextSensor.text = string.Format("Temperatura: {0} {1}\nHumedad: {2} {3}", datos1.Value, datos1.Units, datos2.Value, datos2.Units);
	        }

			Estacion.text = string.Format("{0}", diccionarioNombre[character.PinActual.estacion.ID]);
			flagT = false;
			flagH = false;
    	}
    	else
		{
			TextSensor.text = "";
			Estacion.text = "";
		}
	}
}
