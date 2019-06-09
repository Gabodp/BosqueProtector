using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstVerificationDialog : ShowDialog
{
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            
            if (Clic.clickeados == 3)
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
