using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioDescriptionScript : MonoBehaviour
{
    public string audioName;
    public AudioSource audioSource;
    public AudioClip clip;

    public void playAudio()
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public IEnumerator downloadAudio()
    {
        string url = "/home/rfcx-espol-server/resources/bpv/species/images/" + audioName;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                clip= DownloadHandlerAudioClip.GetContent(www);
            }
        }
        playAudio();
    }
}
