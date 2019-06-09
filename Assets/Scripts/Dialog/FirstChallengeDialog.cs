using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstChallengeDialog : ShowDialog
{
    private GameObject[] interactables;

    void Start()
    {
        interactables = GameObject.FindGameObjectsWithTag("Clic");
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            foreach (GameObject g in interactables)
            {
                g.GetComponent<Clic>().activate = true;
            }
            IntroductionDialog();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().total = 0;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BarChallenge>().instruction.text = "Interactua con especies";

        }
    }
}
