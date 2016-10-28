using UnityEngine;
using System.Collections;

public class LevelTen : MonoBehaviour {
	[HideInInspector]
	public bool isLevelTen=false;


	void OnEnable () 
	{
		isLevelTen=false;
	}
	

	void Update () 
	{
		if (isLevelTen) 
		{

			Transform normalSwitch=transform.Find("switch");
			//GetComponent<PhotoRecognizingPanel> ().ShowFinger(normalSwitch.localPosition);//在开关位置出现小手

//			if (!normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn) //开关闭合
//			{ 
//				if (PhotoRecognizingPanel._instance.finger) 
//				{
//					Destroy (PhotoRecognizingPanel._instance.finger);
//				}
//			} 
			GetImage._instance.cf.switchOnOff (int.Parse (normalSwitch.tag), normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
			CommonFuncManager._instance.CircuitReset (	GetImage._instance.itemList);
		}
	
	}
}
