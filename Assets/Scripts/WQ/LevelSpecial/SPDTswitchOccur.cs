using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SPDTswitchOccur : MonoBehaviour {

	[HideInInspector]
	public bool isSPDTswitchOccur=false;

	private List<GameObject> spdtSwitchList=null;
	private int animationPlayedTimes=0;

	void OnEnable () 
	{
		isSPDTswitchOccur = false;
		animationPlayedTimes=0;
		//spdtSwitchList = PhotoRecognizingPanel._instance.switchList;

	}
	

	void Update () 
	{
		if (isSPDTswitchOccur) 
		{
			spdtSwitchList = PhotoRecognizingPanel._instance.switchList;
			if (!PhotoRecognizingPanel._instance.isArrowShowDone) 
			{

				//在第一个单刀双掷开关出现小手，点击开关闭合，小手消失
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(spdtSwitchList[0].transform.localPosition);

				//默认两个开关都是右闭合
				//如果其中一个左闭合了，电流通
				for (int i = 0; i < spdtSwitchList.Count; i++) 
				{
					if (!spdtSwitchList[i].GetComponent<SPDTswitchCtrl> ().isRightOn) 
					{
						Destroy (PhotoRecognizingPanel._instance.finger);
						CircuitSuccess ();
						GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
						animationPlayedTimes=1;
						break;
					}
				}

			}

			if (animationPlayedTimes == 1) //如果电流播放一次了，
			{

				print (spdtSwitchList[0].GetComponent<SPDTswitchCtrl> ().isRightOn + "+" + spdtSwitchList[1].GetComponent<SPDTswitchCtrl> ().isRightOn);
				//两个都是左闭合或者两个都是右闭合，电流不通
				if (!(spdtSwitchList[0].GetComponent<SPDTswitchCtrl> ().isRightOn ^ spdtSwitchList[1].GetComponent<SPDTswitchCtrl> ().isRightOn)) 
				{

					//中断电流  to do...

					CircuitBreak ();
					Debug.Log("中断电流");

				}
				else 
				{

					//走电流  to do...
					CircuitSuccess();

					Debug.Log("走电流");

				}

			}
		}
	}
		

	private void CircuitSuccess()
	{

		transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbOn";



	}

	private void CircuitBreak()
	{
		transform.Find("bulb").GetComponent<UISprite>().spriteName="bulbOff";
		foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList) {
			//电流应该隐藏而不是销毁    to do ..
			//item.SetActive(false);
			Destroy (item);
		}


	}

}
