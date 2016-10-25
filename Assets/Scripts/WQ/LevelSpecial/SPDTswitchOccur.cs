using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 15
public class SPDTswitchOccur : MonoBehaviour 
{

	[HideInInspector]
	public bool isSPDTswitchOccur=false;

	private List<GameObject> spdtSwitchList=null;
	private int animationPlayedTimes=0;
	private bool isCircuitOpen=false;

	void OnEnable () 
	{
		isSPDTswitchOccur = false;
		animationPlayedTimes=0;
		isCircuitOpen=false;
	}

	void Update () 
	{
		if (isSPDTswitchOccur) 
		{
			spdtSwitchList = PhotoRecognizingPanel._instance.switchList;

			//在第一个单刀双掷开关出现小手，点击开关闭合，小手消失
			GetComponent<PhotoRecognizingPanel> ().ShowFinger(spdtSwitchList[0].transform.localPosition);

			for (int i = 0; i < spdtSwitchList.Count; i++) 
			{
				if (spdtSwitchList[0].GetComponent<SPDTswitchCtrl> ().preStatus !=spdtSwitchList[0].GetComponent<SPDTswitchCtrl> ().isRightOn && PhotoRecognizingPanel._instance.finger) //如果点击了小手指向的开关
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
				}

				//第15关特定
				CurrentFlow_SPDTSwitch._instance.switchOnOff (int.Parse (spdtSwitchList[i].gameObject.tag), spdtSwitchList[i].GetComponent<SPDTswitchCtrl> ().isRightOn ? true : false);
				CommonFuncManager._instance.CircuitReset (CurrentFlow_SPDTSwitch._instance.circuitItems);
			}
		}
	}
}
