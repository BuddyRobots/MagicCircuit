using UnityEngine;
using System.Collections;

public class VOswitchAndLAswitchTogether : MonoBehaviour {


	[HideInInspector]
	public bool isVOswitchAndLAswitchTogether = false;
	private bool isLAswitchOn = false;//光敏开关是否闭合的标志
	private float changeTime = 3f;//渐变的总时间
	private  float changeTimer = 0;
	private int animationPlayedTimes=0;
	private UISprite nightBg=null;
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
		nightBg = PhotoRecognizingPanel._instance.nightBg;
		//UISprite nightBg = PhotoRecognizingPanel._instance.nightBg;
		if (isVOswitchAndLAswitchTogether) 
		{

			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
			
			//在太阳月亮按钮出现小手，点击后变成月亮，小手消失，背景渐变暗，光敏开关在背景变暗之前闭合；在话筒按钮出现小手，点击后小手消失，弹出提示框提示玩家发出声音，收集到声音后提示框消失，声控开关闭合
				//灯亮 走电流

				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("SunAndMoonWidget").localPosition);
				if (!transform.Find ("SunAndMoonWidget").GetComponent<MoonAndSunCtrl> ().isDaytime) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);

					changeTimer += Time.deltaTime;
					if (changeTimer >= changeTime) 
					{
						changeTimer = changeTime;
					}

					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);//蒙版渐变暗
					if(changeTimer>=changeTime/2)//背景渐变到一半的时候
					{
						isLAswitchOn = true;
					}
					if(isLAswitchOn)
					{
						
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";
						//光敏开关闭合，在话筒按钮出现小手

						GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("MicroPhoneBtn").localPosition);
						if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
						{

							Destroy (PhotoRecognizingPanel._instance.finger);
							//弹出提示框，
							PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);

							transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";//声控开关闭合
							transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
							GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
							GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
							animationPlayedTimes=1;//电流播放一次
						}
					}
				
				}
			}


			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{


				//如果点击声控开关，没有反应
				//如果点击光敏开关
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
						isLAswitchOn = false;
					}
					if (!isLAswitchOn) 
					{
						//点击月亮，光敏开关断开，灯泡灭，电流消失
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
						transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOff";
						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOff";
						transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice = false;
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
						isLAswitchOn = true;

					}
					if (isLAswitchOn) 
					{


						transform.Find ("lightActSwitch").GetComponent<UISprite> ().spriteName = "LAswitchOn";

						//需要在话筒按钮出现小手提示玩家
						GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("MicroPhoneBtn").localPosition);
						if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
						{

							Destroy (PhotoRecognizingPanel._instance.finger);
							//弹出提示框，
							PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);

							transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";//声控开关闭合
							transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
							//GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						}
					}
				}






			}





		}
	
	}
}
