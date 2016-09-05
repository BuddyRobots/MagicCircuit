using UnityEngine;
using System.Collections;

public class NormalSwitchOccur : MonoBehaviour {


	[HideInInspector]
	public bool isNormalSwitchOccur = false;



	void Start () 
	{
	
	}
	

	void Update () 
	{
		if (isNormalSwitchOccur) 
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				//在开关位置出现小手，点击开关小手消失，开关闭合，,灯泡变亮，走电流，
				//点击开关断开，电流消失，灯泡变暗
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("switch").localPosition);//出现小手
				//if()
				//{}


			}

		
		}
	
	}
}
