using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarChallenge : MonoBehaviour {

	public Scrollbar bar;
	public float total;
    public Text instruction;
    //public string challenge;

    /*private void Start()
    {
        instruction.text = challenge;
    }*/

    public void Progress(float value)
	{
		total += value;
		bar.size = total / 100f;
	}

}
