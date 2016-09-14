using UnityEngine;
using System.Collections;

//level 6
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
