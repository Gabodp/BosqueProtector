using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondVerificationDialog : ShowDialog
{
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            if (Pick.recogidos == 3)
            {
                disappear = true;
                IntroductionDialog(0);

            }
            else
            {
                IntroductionDialog(1);
            }

        }
    }
}
