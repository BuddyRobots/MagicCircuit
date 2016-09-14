using UnityEngine;
using System.Collections;

public class LevelTen : MonoBehaviour {
	[HideInInspector]
	public bool isLevelTen=false;


	void OnEnable () {
		isLevelTen=false;
	}
	

	void Update () {
		if (isLevelTen) {


			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{
				
			}
		}
	
	}
}
