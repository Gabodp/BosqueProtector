using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour {
	public string url = "http://icons.iconarchive.com/icons/alecive/flatwoken/256/Apps-Gallery-icon.png";
	public RawImage thisImage;

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadFromLinkCoroutine());
	}
	
	private IEnumerator LoadFromLinkCoroutine() {
		Debug.Log("Loading...");
		WWW wwwLoader = new WWW(url);
		yield return wwwLoader;

		Debug.Log("Loaded");
		thisImage.texture = wwwLoader.texture;
	}
}
