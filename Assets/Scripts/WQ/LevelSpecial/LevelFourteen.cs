using UnityEngine;
using System.Collections;
using MagicCircuit;


public class LevelFourteen : MonoBehaviour
{
	[HideInInspector]
	public bool isLevelFourteen = false;
	/// <summary>
	/// 光敏开关是否闭合的标志
	/// </summary>
	private bool isLAswitchOn = false;


	private bool isStartRecord = false;
	private bool isFingerShow = false;
	private bool isFingerDestroyed=false;
	private bool isNightModeOnce=false;
	private bool isCountDownShow=false;
	/// <summary>
	/// 标记太阳月亮按钮的初始状态是显示太阳
	/// </summary>
	private bool preSunSwitchStatues = true;
	private bool CurrLASwitchStatus=false;
	private bool PreLASwitchStatus=false;

	private Transform VoiceDelaySwitch;
	private Transform LAswitch;
	private Transform micphoneBtn;
	private Transform sunMoonBtn;


	private UISprite voiceDelaySprite;
	private UISprite LAswitchSprite;
	private UILabel countDown;

	private PhotoRecognizingPanel photoRecognizePanel;
	private MoonAndSunCtrl moonAndSunCtrl;
	private BoxCollider micPhoneBoxCol;
	private UIButton micPhoneUIBtn;
	private MicroPhoneBtnCtrl micPhontBtnCtrl;


	private  float changeTimer = 0;
	private UITexture nightBg=null;
	//如果玩家先点击了声控开关，声控开关闭合，再点击太阳月亮，光敏开关闭合，线路虽然连通了，也是不行的
	//只有先闭合光敏，再闭合声控，电路才通
	// to do ...
	void OnEnable ()
	{
		countDown = PhotoRecognizingPanel.Instance.countDownLabel;
		CurrLASwitchStatus=false;
		PreLASwitchStatus=false;

		isLevelFourteen = false;
		isLAswitchOn = false;

		changeTimer = 0;
		isFingerShow = false;
		isNightModeOnce=false;
		isFingerDestroyed=false;
		isStartRecord = false;


		isCountDownShow=false;

		VoiceDelaySwitch = transform.Find ("voiceTimedelaySwitch");
		LAswitch = transform.Find ("lightActSwitch");
		micphoneBtn = transform.Find ("MicroPhoneBtn");
		nightBg = PhotoRecognizingPanel.Instance.nightMask;
		sunMoonBtn=transform.Find("SunAndMoonWidget");

		photoRecognizePanel=PhotoRecognizingPanel.Instance;

		moonAndSunCtrl=sunMoonBtn.GetComponent<MoonAndSunCtrl>();
		micPhoneBoxCol = micphoneBtn.gameObject.GetComponent<BoxCollider>();
		micPhoneUIBtn = micphoneBtn.gameObject.GetComponent<UIButton>();
		micPhontBtnCtrl = micphoneBtn.gameObject.GetComponent<MicroPhoneBtnCtrl>();

	}

	//声控+光敏    （只有晚上有声音的时候灯才亮）
	//需要伴随话筒按钮，太阳/月亮按钮出现
	//在太阳月亮按钮出现小手，点击后变成月亮，小手消失，背景渐变暗，光敏开关在背景变暗之前闭合；
	//在话筒按钮出现小手，点击后小手消失，弹出提示框提示玩家发出声音，收集到声音后提示框消失，声控开关闭合
	void Update () 
	{
		if (isLevelFourteen) 
		{
			if (!isFingerShow) 
			{
				photoRecognizePanel.ShowFinger(sunMoonBtn.localPosition);
				isFingerShow=true;
			}

			if (moonAndSunCtrl.isDaytime && !preSunSwitchStatues)//如果是白天，之前的按钮是月亮，说明是从晚上切换到白天
			{
				micphoneBtn.GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice=false;
				isStartRecord = false;
			}
			#region 如果是晚上（点击了太阳按钮）
			if (!moonAndSunCtrl.isDaytime) 
			{ 
				isNightModeOnce = true;
				//销毁小手
				if (!isFingerDestroyed) 
				{
					Destroy (PhotoRecognizingPanel.Instance.finger);
					isFingerShow=false;
					isFingerDestroyed = true;
				}
				//蒙板渐变暗，快全暗时，光敏开关闭合标志打开
				changeTimer += Time.deltaTime;
				if (changeTimer >= Constant.DAYANDNITHT_CHANGETIME) 
				{
					changeTimer = Constant.DAYANDNITHT_CHANGETIME;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / Constant.DAYANDNITHT_CHANGETIME);
				if (changeTimer >= Constant.DAYANDNITHT_CHANGETIME * 5 / 6) 
				{
					isLAswitchOn = true;


					CurrLASwitchStatus=true;
					if (PreLASwitchStatus!=CurrLASwitchStatus) 
					{
						GetImage._instance.cf.switchOnOff (int.Parse (LAswitch.gameObject.tag), true);
						LAswitch.GetComponent<UISprite> ().spriteName = "LAswitchOn";
						PreLASwitchStatus=CurrLASwitchStatus;
					}
				}
				if (isLAswitchOn)//如果光敏开光闭合了
				{	
					micPhoneBoxCol.enabled=true;
					micPhoneUIBtn.enabled=true;
					micPhontBtnCtrl.enabled=true;
					GetComponent<PhotoRecognizingPanel> ().ShowFinger (transform.Find ("MicroPhoneBtn").localPosition);//在话筒按钮出现小手

				}
				if (micphoneBtn.GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice)//点击了话筒按钮
				{ 
					if (photoRecognizePanel.finger) 
					{
						Destroy (photoRecognizePanel.finger);	//小手消失
					}

					if (!isStartRecord) //开始收集声音
					{ 
						photoRecognizePanel.noticeToMakeVoice.SetActive (true);//弹出提示框
						photoRecognizePanel.voiceCollectionMark.SetActive(true);//弹出声音收集图片
						photoRecognizePanel.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = true;//显示声音收集动画
						MicroPhoneInput.getInstance ().StartRecord ();
						isStartRecord = true;
					}
					if (CommonFuncManager._instance.isSoundLoudEnough ()) //收集到声音
					{
						//声控延时开关闭合，倒计时文字出现并开始倒计时

						if (!isCountDownShow) 
						{
							countDown.gameObject.SetActive(true);
							StartCoroutine(CountDown());
							isCountDownShow=true;
						}
						photoRecognizePanel.noticeToMakeVoice.SetActive (false);//提示框消失
						photoRecognizePanel.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = false;
						photoRecognizePanel.voiceCollectionMark.SetActive(false);

						MicroPhoneInput.getInstance().StopRecord();
						GetImage._instance.cf.switchOnOff (int.Parse (VoiceDelaySwitch.gameObject.tag), true);
						VoiceDelaySwitch.GetComponent<UISprite>().spriteName = "VoiceDelayOn";

						CommonFuncManager._instance.CircuitItemRefreshWithOneBattery (GetImage._instance.itemList);
					}
				}
					
			}
			#endregion
			#region 如果是白天
			else 
			{
				if (isNightModeOnce)//白天已经切换到过黑夜了
				{
					StopCoroutine(CountDown());
				
					micphoneBtn.gameObject.GetComponent<BoxCollider>().enabled=false;
					micphoneBtn.gameObject.GetComponent<MicroPhoneBtnCtrl>().enabled=false;

					changeTimer -= Time.deltaTime;
					if (changeTimer <= 0) 
					{
						changeTimer =0;
					}
					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / Constant.DAYANDNITHT_CHANGETIME);
					if (changeTimer <= Constant.DAYANDNITHT_CHANGETIME* 1/6) 
					{
						if (countDown.gameObject.activeSelf)
						{
							countDown.gameObject.SetActive(false);
						}
						CurrLASwitchStatus = false;
						if (PreLASwitchStatus!=CurrLASwitchStatus) 
						{
							GetImage._instance.cf.switchOnOff (int.Parse (LAswitch.gameObject.tag), false);
							LAswitch.GetComponent<UISprite>().spriteName= "LAswitchOff";
							GetImage._instance.cf.switchOnOff (int.Parse (VoiceDelaySwitch.gameObject.tag), false);

							CommonFuncManager._instance.CircuitItemRefreshWithOneBattery (GetImage._instance.itemList);
							VoiceDelaySwitch.GetComponent<UISprite>().spriteName="VoiceDelayOff";
							PreLASwitchStatus=CurrLASwitchStatus;
						}
					}
				}
			}
			#endregion
			preSunSwitchStatues = sunMoonBtn.GetComponent<MoonAndSunCtrl>().isDaytime;
			CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
			}
	}


	IEnumerator CountDown()
	{
		countDown.gameObject.transform.localPosition = VoiceDelaySwitch.localPosition;

		//倒计时，每个数字停留一秒后变化
		yield return new WaitForSeconds(1);
		countDown.text = "4";
		yield return new WaitForSeconds (1);
		countDown.text = "3";
		yield return new WaitForSeconds (1);
		countDown.text = "2";
		yield return new WaitForSeconds (1);
		countDown.text = "1";
		yield return new WaitForSeconds (1);
		countDown.text = " ";


		GetImage._instance.cf.switchOnOff (int.Parse (VoiceDelaySwitch.gameObject.tag), false);
		CommonFuncManager._instance.CircuitItemRefreshWithOneBattery (GetImage._instance.itemList);
		micphoneBtn.GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice = false;	
		VoiceDelaySwitch.GetComponent<UISprite>().spriteName = "VoiceDelayOff";

		isStartRecord = false;
		isCountDownShow=false;

		countDown.gameObject.SetActive (false);
		countDown.text = "5";
	}



}
