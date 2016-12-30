using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第3关
public class LevelThree : MonoBehaviour 
{
	[HideInInspector]
	public bool isNormalSwitchOccur = false;
	public bool  isArrowSemiTrans=true;

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
				if (PhotoRecognizingPanel.Instance.finger) 
				{
					Destroy (PhotoRecognizingPanel.Instance.finger);
				}

			} 
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}

}
