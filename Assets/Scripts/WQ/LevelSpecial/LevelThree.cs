using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第3关
public class LevelThree : MonoBehaviour 
{
	[HideInInspector]
	public bool isNormalSwitchOccur = false;

	void OnEnable()
	{
		isNormalSwitchOccur = false;

		isTest=false;
	}


	private bool isTest=false;

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
				isTest=false;
			} 
			GetImage._instance.cf.switchOnOff (int.Parse (normalSwitch.tag), normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);


			for (int i = 0; i < GetImage._instance.itemList.Count; i++)
			{
				Debug.Log("LevelThree.cs Update : itemList[" + i + "].type = " + GetImage._instance.itemList[i].type +
					" powered = " + GetImage._instance.itemList[i].powered);
			}

//			//for test  ....
//			if (!normalSwitch.GetComponent<SwitchCtrl> ().isSwitchOn && !isTest) 
//			{
//				for (int i = 0; i < PhotoRecognizingPanel._instance.itemList.Count; i++) 
//				{
//					if (PhotoRecognizingPanel._instance.itemList[i].type==ItemType.CircuitLine) 
//					{
//						//Debug.Log("itemlist["+i+"] powered: "+PhotoRecognizingPanel._instance.itemList[i].powered);
//						
//					}
//				}
//				isTest=true;
//
//			}



			CommonFuncManager._instance.CircuitReset (	GetImage._instance.itemList);
		}
	}

}
