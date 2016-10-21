using UnityEngine;
using System.Collections;
using MagicCircuit;

// level 14-------声控延时开关+光敏  
public class LevelFourteen : MonoBehaviour
{


	[HideInInspector]
	public bool isLevelFourteen = false;
	private UITexture nightBg=null;
	private float changeTime = 3f;//渐变的总时间
	private  float changeTimer = 0;
	private int animationPlayedTimes=0;
	private bool isLAswitchOn = false;//光敏开关是否闭合的标志

	/// <summary>
	/// 声控开关闭合一次的总时间
	/// </summary>
	private float VOTime=5f;
	private float VOTimer=0;

	private bool isStartRecord = false;

	void OnEnable () 
	{
		isLevelFourteen = false;
		isLAswitchOn = false;
		changeTime = 3f;
		changeTimer = 0;
		animationPlayedTimes=0;
	}
	

	void Update () 
	{
		if (isLevelFourteen) 
		{
			Transform LAswitch = transform.Find ("lightActSwitch");
			Transform VoiceDelaySwitch = transform.Find ("voiceTimedelaySwitch");

			nightBg = PhotoRecognizingPanel._instance.nightMask;
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("SunAndMoonWidget").localPosition);
				if (!transform.Find ("SunAndMoonWidget").GetComponent<MoonAndSunCtrl> ().isDaytime) //如果点击了太阳，是晚上模式
				{
				
					Destroy (PhotoRecognizingPanel._instance.finger);//小手消失

					changeTimer += Time.deltaTime;
					if (changeTimer >= changeTime) 
					{
						changeTimer = changeTime;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);//蒙板渐变暗
					if(changeTimer>=changeTime*5/6)//蒙板快变暗时
					{
						isLAswitchOn = true;//光敏开关闭合
					}
					if(isLAswitchOn)
					{
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";//光敏开关闭合
						GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);//在话筒按钮出现小手
						if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
						{

							Destroy (PhotoRecognizingPanel._instance.finger);
							PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);//弹出提示框，
							if (!isStartRecord) 
							{
								MicroPhoneInput.getInstance().StartRecord();
								isStartRecord = true;
							}
							if(CommonFuncManager._instance.isSoundLoudEnough())
							{
								MicroPhoneInput.getInstance().StopRecord();
								CircuitOpen ();
								GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
								animationPlayedTimes=1;//电流播放一次

								VOTimer += Time.deltaTime;
								if (VOTimer >= VOTime) 
								{
								
									VOTimer = 0;
									//电路断开   to do ...
									//声控延时开关断开，灯变暗
									transform.Find("voiceTimedelaySwitch").GetComponent<UISprite>().spriteName="voiceTimedelaySwitchOff";
									transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
									transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice = false;
								}

							}

						}
					}
				}
			}

			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				CommonFuncManager._instance.CircuitOnOrOff(isLAswitchOn);

				//如果点击声控开关，没有反应 to do...
				if (transform.Find("SunAndMoonWidget").GetComponent<MoonAndSunCtrl>().isDaytime)//如果点击光敏开关
				{	//如果是白天
					changeTimer -= Time.deltaTime;
					if (changeTimer <= 0) 
					{
						changeTimer =0;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
					if (changeTimer <= changeTime* 1/6) 
					{
						isLAswitchOn = false;
					}
					if (!isLAswitchOn) 
					{//如果光敏开关断开，则电路断开
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
						transform.Find("voiceTimedelaySwitch").GetComponent<UISprite>().spriteName="voiceTimedelaySwitchOn";
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOff";
						transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice = false;
						isStartRecord = false;

					}
				}
				else
				{//如果是晚上
					changeTimer += Time.deltaTime;
					if (changeTimer >= changeTime) 
					{
						changeTimer = changeTime;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);

					if(changeTimer>=changeTime*5/6)
					{
						isLAswitchOn = true;

					}
					if (isLAswitchOn) //光敏开关闭合
					{
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";
						GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);//需要在话筒按钮出现小手提示玩家
						if (!transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
						{
							Destroy (PhotoRecognizingPanel._instance.finger);
							//弹出提示框，
							PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);
							if (!isStartRecord) 
							{
								MicroPhoneInput.getInstance().StartRecord();
								isStartRecord = true;
							}
							if(CommonFuncManager._instance.isSoundLoudEnough())
							{
								MicroPhoneInput.getInstance().StopRecord();
								PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
								transform.Find("voiceTimedelaySwitch").GetComponent<UISprite>().spriteName="voiceTimedelaySwitchOn";
								transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
								VOTimer += Time.deltaTime;
								if (VOTimer >= VOTime) 
								{

									VOTimer = 0;
									//电路断开   to do ...
									//声控延时开关断开，灯变暗
									transform.Find("voiceTimedelaySwitch").GetComponent<UISprite>().spriteName="voiceTimedelaySwitchOff";
									transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
									transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice = false;
								}
							}
						}
					}
				}
			}

		}
	
	}

	private void CircuitOpen()
	{
		PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
		transform.Find("voiceTimedelaySwitch").GetComponent<UISprite>().spriteName="voiceTimedelaySwitchOn";//声控开关闭合
		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
		GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
	}
}
