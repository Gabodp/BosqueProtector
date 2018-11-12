using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MouseController : MonoBehaviour {

	private Rect screenRect;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
	}

	/*void Update(){
		if (!screenRect.Contains(Input.mousePosition)) {
			GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
		} else {
			GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
		}
	}*/
	
	void OnApplicationFocus(bool ApplicationIsBack){
		if (ApplicationIsBack == true){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}
