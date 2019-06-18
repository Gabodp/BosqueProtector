using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengePass2 : MonoBehaviour
{
    private void Update()
    {
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
