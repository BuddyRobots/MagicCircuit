using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;

//level 15
public class LevelFifteen : MonoBehaviour 
{

	[HideInInspector]
	public bool isSPDTswitchOccur=false;

	private List<GameObject> spdtSwitchList=null;

	void OnEnable () 
	{
		isSPDTswitchOccur = false;
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
					
				CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);

			}
		}
	}
}
