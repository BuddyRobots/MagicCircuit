using UnityEngine;
using System.Collections;

public class LabelCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void SetBatteryThreshold(string name)
	{


	}


	public void SetThreshold(string name)
	{

		switch (name) 
		{
			case "battery":
			
				break;
			case "light":
				break;
			case "switch":
				break;
			case "line":
				break;
				
			default:
				break;
		}
	}

}
