using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//level 15
public class SPDTswitchOccur : MonoBehaviour {

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
			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{
				//在第一个单刀双掷开关出现小手，点击开关闭合，小手消失
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(spdtSwitchList[0].transform.localPosition);

				for (int i = 0; i < spdtSwitchList.Count; i++) 
				{
					if (!spdtSwitchList[i].GetComponent<SPDTswitchCtrl> ().isRightOn) //默认两个开关都是右闭合,如果其中一个左闭合了，电流通
					{
						if (i==0) 
						{
							Destroy (PhotoRecognizingPanel._instance.finger);
						}
						CommonFuncManager._instance.OpenCircuit ();
						animationPlayedTimes=1;
						break;
					}
				}

			}
			if (animationPlayedTimes == 1) //如果电流播放一次了，
			{
				CommonFuncManager._instance.CircuitOnOrOff (isCircuitOpen);
				//两个都是左闭合或者两个都是右闭合，电流不通
				if (!(spdtSwitchList[0].GetComponent<SPDTswitchCtrl> ().isRightOn ^ spdtSwitchList[1].GetComponent<SPDTswitchCtrl> ().isRightOn)) 
				{
					transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbOff";
					isCircuitOpen = false;
					//Debug.Log("中断电流");
				}
				else 
				{
					isCircuitOpen = true;
					transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbOn";
					//Debug.Log("走电流");
				}
			}
		}
	}
}
