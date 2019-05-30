using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengePass : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(Pick.recogidos);
        if (Pick.recogidos == 3)
        {
            this.GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            this.GetComponent<Collider>().isTrigger = false;
        }
    }
}
