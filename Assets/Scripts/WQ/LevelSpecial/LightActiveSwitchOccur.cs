using UnityEngine;
using System.Collections;

public class LightActiveSwitchOccur : MonoBehaviour {


	private int animationPlayedTimes=0;
	[HideInInspector]
	public bool isLAswitchOccur = false;

	private UISprite nightBg=null;
	private float changeTime = 3f;//渐变的总时间
	private  float changeTimer = 0;
	private bool isCircuitWork = false;

	void OnEnable () 
	{
		nightBg = PhotoRecognizingPanel._instance.nightBg;
		changeTime = 3f;
		changeTimer = 0;

		animationPlayedTimes=0;
		isLAswitchOccur = false;
		isCircuitWork = false;
	}

	void Update () 
	{
		if (isLAswitchOccur)
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
			//在太阳月亮按钮位置出现小手，点击太阳，蒙版渐变暗，小手消失，光敏开关闭合，灯泡亮，电流走起
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("SunAndMoonWidget").localPosition);
				if(!transform.Find("SunAndMoonWidget").GetComponent<MoonAndSunCtrl>().isDaytime)
				{//如果是晚上
					changeTimer += Time.deltaTime;
					if (changeTimer >= changeTime) 
					{
						changeTimer = changeTime;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);//蒙版渐变暗
					if(changeTimer>=changeTime/2)//背景渐变到一半的时候
					{
						isCircuitWork = true;
					}

					Destroy (PhotoRecognizingPanel._instance.finger);

					if(isCircuitWork)
					{
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";
						GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
						animationPlayedTimes=1;//电流播放一次
					}
				}
			}

			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				if (transform.Find("SunAndMoonWidget").GetComponent<MoonAndSunCtrl>().isDaytime)
				{//如果是白天
					
					changeTimer -= Time.deltaTime;
					if (changeTimer <= 0) 
					{
						changeTimer =0;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
					if (changeTimer <= changeTime / 2) 
					{
						isCircuitWork = false;
					}
					if (!isCircuitWork) 
					{
						//点击月亮，光敏开关断开，灯泡灭，电流消失
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbDark";
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOff";
						foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList)
						{
							Destroy(item);
							//item.GetComponent<UISprite>().alpha=0;
						}
					}
				}
				else 
				{//如果是晚上
					//蒙版渐变暗  
					changeTimer += Time.deltaTime;
					if (changeTimer >= changeTime) {

						changeTimer = changeTime;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);

					if(changeTimer>=changeTime/2)
					{
						isCircuitWork = true;

					}
					if (isCircuitWork) 
					{

						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";


					}
				}

			}
		}
	}
}
