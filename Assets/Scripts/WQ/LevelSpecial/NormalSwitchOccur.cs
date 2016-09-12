using UnityEngine;
using System.Collections;

public class NormalSwitchOccur : MonoBehaviour {


	[HideInInspector]
	public bool isNormalSwitchOccur = false;

	private int animationPlayedTimes=0;

	void OnEnable()
	{
		animationPlayedTimes=0;

	}

	void Update () 
	{
		if (isNormalSwitchOccur) 
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("switch").localPosition);//在开关位置出现小手

				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
					CommonFuncManager._instance.OpenCircuit ();
					animationPlayedTimes=1;//电流播放一次
				}	
			}

			if (animationPlayedTimes==1) //如果已经播放过电流
			{

				CommonFuncManager._instance.CircuitOnOrOff (!transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn);
				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn)//开关断开
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
//					foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
//					{
//						//电流应该停止走动，并隐藏，而不是销毁..to do ..
//						//item.SetActive(false);
//						Destroy(item);
//					}

				}
				else 
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
				}

			}

		}
	}
}
