using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第3关
public class NormalSwitchOccur : MonoBehaviour 
{
	[HideInInspector]
	public bool isNormalSwitchOccur = false;

	void OnEnable()
	{
		isNormalSwitchOccur = false;
	}

	void Update () 
	{
		if (isNormalSwitchOccur) 
		{
			Transform normalSwitch=transform.Find("switch");
			GetComponent<PhotoRecognizingPanel> ().ShowFinger(normalSwitch.localPosition);//在开关位置出现小手

			if (!normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn) //开关闭合
			{ 
				if (PhotoRecognizingPanel._instance.finger) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
				}
			} 
			CurrentFlow._instance.switchOnOff (int.Parse (normalSwitch.tag), normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
			CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);
		}
	}

}
