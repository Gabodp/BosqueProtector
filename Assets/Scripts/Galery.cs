using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Galery : MonoBehaviour {


	public Sprite[] imagenes;
	public int imagenActual = 0;
	public GameObject imagen;


	// Use this for initialization
	void Start () {
		CargarImagen(0);
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp("left"))
        {
			CargarImagen(1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp("left"))
        {
			CargarImagen(-1);
        }
	}
	
	public void CargarImagen (int num) {
		imagenActual += num;
		if (imagenActual >= imagenes.Length) {
			imagenActual = 0;
		}
		else if (imagenActual < 0) {
			imagenActual = 0;
		}
		imagen.GetComponent<Image>().sprite = imagenes[imagenActual];
	}
}
