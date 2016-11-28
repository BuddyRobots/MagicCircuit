using UnityEngine;
using System.Collections;
using MagicCircuit;

//第12关----光敏开关
public class LevelTwelve : MonoBehaviour 
{
	[HideInInspector]
	public bool isLAswitchOccur = false;

	private  float dayAndNight_changeTimer = 0;
	private bool isFingerShow = false;
	private bool isFingerDestroyed=false;
	private bool CurrLASwitchStatus=false;
	private bool PreLASwitchStatus=false;

	private Transform LAswitch;
	private UITexture nightBg=null;

	void OnEnable () 
	{
		dayAndNight_changeTimer = 0;
		isLAswitchOccur = false;

		isFingerShow = false;
		isFingerDestroyed=false;

		LAswitch = transform.Find ("lightActSwitch");
		nightBg = PhotoRecognizingPanel._instance.nightMask;

	}



	void Update () 
	{
		if (isLAswitchOccur)
		{
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
				dayAndNight_changeTimer += Time.deltaTime;
				if (dayAndNight_changeTimer >= Constant.DAYANDNITHT_CHANGETIME) 
				{
					dayAndNight_changeTimer = Constant.DAYANDNITHT_CHANGETIME;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, dayAndNight_changeTimer / Constant.DAYANDNITHT_CHANGETIME);//蒙版渐变暗
				if(dayAndNight_changeTimer>=Constant.DAYANDNITHT_CHANGETIME*5/6)//背景渐变快完成时
				{
					CurrLASwitchStatus=true;
					if (PreLASwitchStatus!=CurrLASwitchStatus) 
					{
						GetImage._instance.cf.switchOnOff (int.Parse (LAswitch.gameObject.tag), true);
						LAswitch.GetComponent<UISprite>().spriteName= "LAswitchOn";
						CommonFuncManager._instance.CircuitItemRefreshWithOneBattery (GetImage._instance.itemList);
						PreLASwitchStatus=CurrLASwitchStatus;
					}
				}
			}
			else //如果是白天
			{
				dayAndNight_changeTimer -= Time.deltaTime;
				if (dayAndNight_changeTimer <= 0) 
				{
					dayAndNight_changeTimer =0;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, dayAndNight_changeTimer / Constant.DAYANDNITHT_CHANGETIME);
				if (dayAndNight_changeTimer <= Constant.DAYANDNITHT_CHANGETIME / 6) 
				{
					CurrLASwitchStatus = false;
					if (PreLASwitchStatus!=CurrLASwitchStatus) 
					{
						GetImage._instance.cf.switchOnOff (int.Parse (LAswitch.gameObject.tag), false);
						LAswitch.GetComponent<UISprite>().spriteName= "LAswitchOff";
						CommonFuncManager._instance.CircuitItemRefreshWithOneBattery (GetImage._instance.itemList);
						PreLASwitchStatus=CurrLASwitchStatus;
					}
				}
			}	
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
		}
	}

}
