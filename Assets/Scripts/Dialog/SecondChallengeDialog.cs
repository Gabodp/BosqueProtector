using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChallengeDialog : ShowDialog
{
    private GameObject[] recolectables;

    void Start()
    {
        recolectables = GameObject.FindGameObjectsWithTag("Pick");
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            foreach (GameObject g in recolectables)
            {
                g.GetComponent<Pick>().activate = true;
            }
            IntroductionDialog();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().total = 0; 
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().instruction.text = "Encuentra ratones o salamandras"; 
        }
    }
}
