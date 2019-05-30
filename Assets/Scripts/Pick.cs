using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    public bool activate = false;
    public static int recogidos; 

    private void OnMouseDown()
    {
        if (activate)
        {
            recogidos += 1;
            Debug.Log(recogidos);
            Destroy(this.gameObject);
        }
    }


}
