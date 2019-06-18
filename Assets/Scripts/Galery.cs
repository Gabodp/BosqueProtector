using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using ArbolClass;

public class Galery : MonoBehaviour {

	public string info_url;
	public string image_url;
	public int treeID;
	public Text titulo;
	public Text cuerpo;
	public Sprite[] imagenes;
	public int imagenActual = 0;
	public RawImage imagen;
    public GameObject audioDescriptionButton;

	private Arbol tree = null;
	public bool visible;

	void Update () {
		if (visible) {
			StartCoroutine(CargarInfo());
			visible = false;
		}

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

        //Se manda el nombre del audiodescripcion al que apunta el galleryItem actual.
        audioDescriptionButton.GetComponent<AudioDescriptionScript>().audioName = tree.Gallery[imagenActual].audioname;
		cuerpo.text = tree.Gallery[imagenActual].Description;
		StartCoroutine(LoadImage(tree.Gallery[imagenActual].Id));
	}

	public IEnumerator CargarInfo() {

		UnityWebRequest www = UnityWebRequest.Get(info_url + name);
		Debug.Log("URL de la info:" + info_url + name);
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
		CargarImagen(0);
	}

	public IEnumerator LoadImage(int id) {

        UnityWebRequest w = UnityWebRequestTexture.GetTexture(image_url + tree.Id + "/gallery/" + id + "/");
        Debug.Log("URL de la imagen: " + image_url + treeID + "/gallery/" + id + "/");
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


