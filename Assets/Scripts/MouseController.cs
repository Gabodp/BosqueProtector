using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MouseController : MonoBehaviour {

	private Rect screenRect;

	public FirstPersonController fpsController;
	public float range = 100f;
	public Camera fpsCamera;

	private ClickMouse click;

	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		screenRect = new Rect(0,0, Screen.width, Screen.height);
	}

	void Update(){
		if (!GameManager.instance.paused){
			if (screenRect.Contains(Input.mousePosition)) {
				fpsController.enabled = true;
			} else {
				fpsController.enabled = false;
			}
		}

		if (Input.GetButtonDown("Fire1")) {
			ClickObject();
		}
	}
	
	void OnApplicationFocus(bool ApplicationIsBack){
		if (ApplicationIsBack == true){
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void ClickObject() {
		RaycastHit hit;
		if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range)) {
			click = hit.collider.gameObject.GetComponent<ClickMouse>();
			if (click != null) {
				click.ShowGallery();
			}
		};
	}
}
