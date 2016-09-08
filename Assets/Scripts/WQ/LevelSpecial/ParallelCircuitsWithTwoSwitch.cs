using UnityEngine;
using System.Collections;

public class ParallelCircuitsWithTwoSwitch : MonoBehaviour {
	[HideInInspector]
	public bool isParrallelCircuit = false;



	void OnEnable ()
	{
		isParrallelCircuit = false;
	}
	

	void Update () 
	{
		if (isParrallelCircuit) 
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
			
			


			}

			
		}
	}
}
