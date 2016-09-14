using UnityEngine;
using System.Collections;


//level 13
public class VOswitchAndLAswitchTogether : MonoBehaviour
{

	[HideInInspector]
	public bool isVOswitchAndLAswitchTogether = false;
	private bool isLAswitchOn = false;//光敏开关是否闭合的标志
	private float changeTime = 3f;//渐变的总时间
	private  float changeTimer = 0;
	private int animationPlayedTimes=0;
	private UITexture nightBg=null;

	private bool isStartRecord = false;
	//  ????????????
	//如果玩家先点击了声控开关，声控开关闭合，再点击太阳月亮，光敏开关闭合，线路虽然连通了，也是不行的
	//只有先闭合光敏，再闭合声控，电路才通
	// to do ...


	void OnEnable ()
	{
		
		isVOswitchAndLAswitchTogether = false;
		isLAswitchOn = false;
		changeTime = 3f;
		changeTimer = 0;
		animationPlayedTimes=0;
	
	}
	
	//声控+光敏    （只有晚上有声音的时候灯才亮）//需要伴随话筒按钮，太阳/月亮按钮出现
	void Update () 
	{
		if (isVOswitchAndLAswitchTogether) 
		{
			nightBg = PhotoRecognizingPanel._instance.nightMask;

			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				
			
			//在太阳月亮按钮出现小手，点击后变成月亮，小手消失，背景渐变暗，光敏开关在背景变暗之前闭合；在话筒按钮出现小手，点击后小手消失，弹出提示框提示玩家发出声音，收集到声音后提示框消失，声控开关闭合
				//灯亮 走电流

				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("SunAndMoonWidget").localPosition);//在太阳月亮按钮出现小手
				if (!transform.Find ("SunAndMoonWidget").GetComponent<MoonAndSunCtrl> ().isDaytime) //点击按钮，切换到夜晚模式
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
						GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("MicroPhoneBtn").localPosition);//在话筒按钮出现小手
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
						transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOff";
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
						GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("MicroPhoneBtn").localPosition);//需要在话筒按钮出现小手提示玩家
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
								transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";
								transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
							}

							//print ("++++++ " + GetComponent<PhotoRecognizingPanel> ().arrowList.Count);

							//GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						}
					}
				}
			}
		}
	}


	private void CircuitOpen()
	{
		PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
		transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";//声控开关闭合
		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
		GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);


	}

//	private bool tempIsLaSwitch = false;
//
//	private void CircuitOnOrOff(bool isSwitch)
//	{
//		if (tempIsLaSwitch != isSwitch) 
//		{
//			if (!isSwitch) 
//			{
//				GetComponent<PhotoRecognizingPanel> ().StopCircuit ();
//			}
//			else
//			{
//				GetComponent<PhotoRecognizingPanel> ().ContinueCircuit ();
//			}
//			tempIsLaSwitch = isSwitch;
//		} 
//	}
}
