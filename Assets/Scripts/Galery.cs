using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Galery : MonoBehaviour {

	public string info_url;
	public string image_url;
	public int treeID;
	public Text titulo;
	public Text cuerpo;
	public Sprite[] imagenes;
	public int imagenActual = 0;
	public RawImage imagen;

	private Arbol tree = null;


	// Use this for initialization
	void Start () {
		StartCoroutine(CargarInfo());
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
        {
			CargarImagen(1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D))
        {
			CargarImagen(-1);
        }
	}
	
	public void CargarImagen (int num) {
		imagenActual += num;
		if (imagenActual >= tree.Gallery.Length) {
			imagenActual = 0;
		}
		else if (imagenActual < 0) {
			imagenActual = 0;
		}

		cuerpo.text = tree.Gallery[imagenActual].Description;
		StartCoroutine(LoadImage(tree.Gallery[imagenActual].Id));
	}

	public IEnumerator CargarInfo() {

		UnityWebRequest www = UnityWebRequest.Get(info_url + treeID + "/");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		} else {
			string data = www.downloadHandler.text;
			try {
				tree = JsonUtility.FromJson<Arbol>(data);
			} catch (System.Exception) {
				Debug.Log("No hay datos de este árbol");
			}
		}

		titulo.text = tree.Name;
		Debug.Log(tree.Gallery.Length);
		CargarImagen(0);
	}

	public IEnumerator LoadImage(int id) {
		WWW wwwLoader = new WWW(image_url + treeID + "/gallery/" + id + "/");
		Debug.Log(image_url + treeID + "/gallery/" + id + "/");
		yield return wwwLoader;

		imagen.texture = wwwLoader.texture;
	}
}

[Serializable]
public class Arbol {
	public string SpecieId;
	public int Id;
	public string Name;
	public string Family;
	public Gallery[] Gallery;
}

[Serializable]
public class Gallery {
	public string PhotoId;
	public int Id;
	public string Description;
}
