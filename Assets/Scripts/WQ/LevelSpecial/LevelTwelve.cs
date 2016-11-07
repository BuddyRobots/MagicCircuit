using UnityEngine;
using System.Collections;
using MagicCircuit;

//第12关----光敏开关
public class LevelTwelve : MonoBehaviour 
{
	[HideInInspector]
	public bool isLAswitchOccur = false;

	private UITexture nightBg=null;
	private float changeTime = 3f;//渐变的总时间
	private  float changeTimer = 0;
	private bool isCircuitWork = false;
	private bool isFingerShow = false;
	private bool isFingerDestroyed=false;




	void OnEnable () 
	{
		changeTime = 3f;
		changeTimer = 0;
		isLAswitchOccur = false;
		isCircuitWork = false;

		isFingerShow = false;
		isFingerDestroyed=false;
	}

	void Update () 
	{
		if (isLAswitchOccur)
		{
			Transform LAswitch = transform.Find ("lightActSwitch");
			nightBg = PhotoRecognizingPanel._instance.nightMask;
			if (!isFingerShow) 
			{
				//在太阳月亮按钮位置出现小手，点击太阳，蒙版渐变暗，小手消失，光敏开关闭合，灯泡亮，电流走起
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("SunAndMoonWidget").localPosition);
				isFingerShow = true;
			}
			if(!transform.Find("SunAndMoonWidget").GetComponent<MoonAndSunCtrl>().isDaytime)//如果是晚上
			{
				if (!isFingerDestroyed) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);	
					isFingerDestroyed = true;
				}
				changeTimer += Time.deltaTime;
				if (changeTimer >= changeTime) 
				{
					changeTimer = changeTime;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);//蒙版渐变暗
				if(changeTimer>=changeTime*5/6)//背景渐变到一半的时候
				{
					isCircuitWork = true;
				}
			}
			else //如果是白天
			{
				changeTimer -= Time.deltaTime;
				if (changeTimer <= 0) 
				{
					changeTimer =0;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
				if (changeTimer <= changeTime / 6) 
				{
					isCircuitWork = false;
				}
			}	
			GetImage._instance.cf.switchOnOff (int.Parse (LAswitch.gameObject.tag), isCircuitWork);
			LAswitch.GetComponent<UISprite>().spriteName=isCircuitWork? "LAswitchOn":"LAswitchOff";
			CommonFuncManager._instance.CircuitReset (GetImage._instance.itemList);	
		}
	}


}
