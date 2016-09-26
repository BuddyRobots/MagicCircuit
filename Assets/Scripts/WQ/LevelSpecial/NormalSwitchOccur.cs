using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第3关
public class NormalSwitchOccur : MonoBehaviour 
{

	private List<GameObject> normalSwitchList =null;
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


			normalSwitchList = GetComponent<PhotoRecognizingPanel> ().switchList;
			GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("switch").localPosition);//在开关位置出现小手

			if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn == false) //开关闭合
			{ 
				if (PhotoRecognizingPanel._instance.finger) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
					Debug.Log ("finger destroyed");
				}
				CurrentFlow._instance.switchOnOff (int.Parse (transform.Find ("switch").tag), true );
			} 
			else 
			{
				CurrentFlow._instance.switchOnOff (int.Parse (transform.Find ("switch").tag), false);
			}




			//CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);

//			for (int i = 0; i < normalSwitchList.Count; i++) 
//			{
//				CurrentFlow._instance.switchOnOff (int.Parse (normalSwitchList [i].tag), normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
//
//				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);
//
//			}


//			else 
//			{
//			
//				transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
//			
//			}



			//CurrentFlow._instance.switchOnOff (int.Parse(transform.Find("switch").gameObject.tag), transform.Find("switch").GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);


//			for (int i = 0; i < normalSwitchList.Count; i++) 
//			{
//				CurrentFlow._instance.switchOnOff (int.Parse(normalSwitchList [i].tag), normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);
//
//				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);
//
//			}


//			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
//			{
//				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("switch").localPosition);//在开关位置出现小手
//
//				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
//				{
//					Destroy (PhotoRecognizingPanel._instance.finger);
//					CommonFuncManager._instance.OpenCircuit ();
//					animationPlayedTimes=1;//电流播放一次
//				}	
//			}
//
//			if (animationPlayedTimes==1) //如果已经播放过电流
//			{
//
				CommonFuncManager._instance.CircuitOnOrOff (!transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn);
//				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn)//开关断开
//				{
//					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
////					foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
////					{
////						//电流应该停止走动，并隐藏，而不是销毁..to do ..
////						//item.SetActive(false);
////						Destroy(item);
////					}
//
//				}
//				else 
//				{
//					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
//				}
//
//			}

		}
	}

}
