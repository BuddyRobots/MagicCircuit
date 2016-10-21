//using UnityEngine;
//using System.Collections;
//using MagicCircuit;
////level 13---声控+光敏
//
//public class VOswitchAndLAswitchTogether : MonoBehaviour
//{
//	[HideInInspector]
//	public bool isVOswitchAndLAswitchTogether = false;
//	/// <summary>
//	/// 光敏开关是否闭合的标志
//	/// </summary>
//	private bool isLAswitchOn = false;
//
//	private float changeTime = 3f;//渐变的总时间
//	private  float changeTimer = 0;
//	private int animationPlayedTimes=0;
//	private UITexture nightBg=null;
//	private bool isAnimationPlay=false;
//	private bool isStartRecord = false;
//	private bool isFingerShow = false;
//	private bool isFingerShowTwice=false;
//	private bool isFingerDestroyed=false;
//	private bool isNightModeOnce=false;
//	//  ????????????
//	//如果玩家先点击了声控开关，声控开关闭合，再点击太阳月亮，光敏开关闭合，线路虽然连通了，也是不行的
//	//只有先闭合光敏，再闭合声控，电路才通
//	// to do ...
//	void OnEnable ()
//	{
//		isVOswitchAndLAswitchTogether = false;
//		isLAswitchOn = false;
//		changeTime = 3f;
//		changeTimer = 0;
//		animationPlayedTimes=0;
//		isAnimationPlay=false;
//		isFingerShow = false;
//		isFingerShowTwice=false;
//		isNightModeOnce=false;
//		isFingerDestroyed=false;
//	}
//	
//	//声控+光敏    （只有晚上有声音的时候灯才亮）
//	//需要伴随话筒按钮，太阳/月亮按钮出现
//	//在太阳月亮按钮出现小手，点击后变成月亮，小手消失，背景渐变暗，光敏开关在背景变暗之前闭合；
//	//在话筒按钮出现小手，点击后小手消失，弹出提示框提示玩家发出声音，收集到声音后提示框消失，声控开关闭合
//	void Update () 
//	{
//		if (isVOswitchAndLAswitchTogether) 
//		{
//			Transform LAswitch = transform.Find ("lightActSwitch");
//			Transform VOswitch = transform.Find ("voiceOperSwitch");
//
//			nightBg = PhotoRecognizingPanel._instance.nightMask;
//
//			//在太阳月亮按钮出现小手
//			if (!isFingerShow) 
//			{
//				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("SunAndMoonWidget").localPosition);
//				isFingerShow=true;
//			}
//			#region 如果是晚上（点击了太阳按钮）
//			if (!transform.Find ("SunAndMoonWidget").GetComponent<MoonAndSunCtrl> ().isDaytime) 
//			{ 
//				isNightModeOnce = true;
//				//销毁小手
//				if (!isFingerDestroyed) 
//				{
//					Destroy (PhotoRecognizingPanel._instance.finger);
//					isFingerDestroyed = true;
//				}
//				//蒙板渐变暗，快全暗时，光敏开关闭合标志打开
//				changeTimer += Time.deltaTime;
//				if (changeTimer >= changeTime) 
//				{
//					changeTimer = changeTime;
//				}
//				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
//				if (changeTimer >= changeTime * 5 / 6) 
//				{
//					isLAswitchOn = true;
//
//				}
////				if (isLAswitchOn)
////				{		
////					Debug.Log("isLAswitchOn===="+isLAswitchOn);
////					Debug.Log("before name====="+LAswitch.GetComponent<UISprite> ().spriteName);
////					LAswitch.GetComponent<UISprite> ().spriteName ="LAswitchOn";//光敏开关闭合
////					Debug.Log("after name====="+LAswitch.GetComponent<UISprite> ().spriteName);
////
////					//只有晚上光敏开关闭合的时候，小话筒才可以被点击
////					transform.Find("MicroPhoneBtn").gameObject.GetComponent<UIButton>().enabled=true;
////					transform.Find("MicroPhoneBtn").gameObject.GetComponent<MicroPhoneBtnCtrl>().enabled=true;
////					GetComponent<PhotoRecognizingPanel> ().ShowFinger (transform.Find ("MicroPhoneBtn").localPosition);//在话筒按钮出现小手
////					isFingerDestroyed = false;
////				}
//
////				//光敏开关闭合    
////				CurrentFlow._instance.switchOnOff (int.Parse (LAswitch.gameObject.tag), isLAswitchOn);
//	
////				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice)//点击了话筒按钮
////				{ 
////					if (!isFingerDestroyed) 
////					{
////						Destroy (PhotoRecognizingPanel._instance.finger);	//小手消失
////						isFingerDestroyed = true;
////					}
////					PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive (true);//弹出提示框
////					if (!isStartRecord) //开始收集声音
////					{ 
////						MicroPhoneInput.getInstance ().StartRecord ();
////						isStartRecord = true;
////					}
////					if (CommonFuncManager._instance.isSoundLoudEnough ()) //收集到声音
////					{
////						
////						VOswitch.GetComponent<UISprite> ().spriteName = "LAswitchOn";//光敏开关闭合
////						GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);//在话筒按钮出现小手
////						if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
////						{
////
////							Destroy (PhotoRecognizingPanel._instance.finger);
////							PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);//弹出提示框，
////							if (!isStartRecord) 
////							{
////								MicroPhoneInput.getInstance().StartRecord();
////								isStartRecord = true;
////							}
////							if(CommonFuncManager._instance.isSoundLoudEnough())
////							{
////								MicroPhoneInput.getInstance().StopRecord();
////								CircuitOpen ();
////								GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
////					
////							}
////
////						}
////					}
////				}
//				//CurrentFlow._instance.switchOnOff (int.Parse (VOswitch.gameObject.tag), isAnimationPlay);
//				//CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);
//			}
//			#endregion
//			#region 如果是白天
//			else 
//			{
//				transform.Find("MicroPhoneBtn").gameObject.GetComponent<UIButton>().enabled=false;
//				transform.Find("MicroPhoneBtn").gameObject.GetComponent<MicroPhoneBtnCtrl>().enabled=false;
//				//if (isNightModeOnce) 
//				{
//					//isNightModeOnce=false;
//
//					changeTimer -= Time.deltaTime;
//					if (changeTimer <= 0) 
//					{
//						changeTimer =0;
//					}
//					nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
//					if (changeTimer <= changeTime* 1/6) 
//					{
//						isLAswitchOn = false;
////						CurrentFlow._instance.switchOnOff (int.Parse (VOswitch.gameObject.tag), false);
////						CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);
//					}	
//
//				}
//
//			}
//			#endregion
//			CurrentFlow._instance.switchOnOff (int.Parse (VOswitch.gameObject.tag), isLAswitchOn);
//			CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);	
//		}
//	}
//
//
//	private void CircuitOpen()
//	{
//		PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
//		transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";//声控开关闭合
//		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
//		GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
//	}
//		
//}










using UnityEngine;
using System.Collections;
using MagicCircuit;

//第12关----光敏开关
public class VOswitchAndLAswitchTogether : MonoBehaviour 
{
	private int animationPlayedTimes=0;
	[HideInInspector]
	public bool isVOswitchAndLAswitchTogether = false;

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

		animationPlayedTimes=0;
		isVOswitchAndLAswitchTogether = false;
		isCircuitWork = false;

		isFingerShow = false;
		isFingerDestroyed=false;
	}

	void Update () 
	{
		if (isVOswitchAndLAswitchTogether) {
			Transform LAswitch = transform.Find ("lightActSwitch");
			nightBg = PhotoRecognizingPanel._instance.nightMask;
			if (!isFingerShow) {
				//在太阳月亮按钮位置出现小手，点击太阳，蒙版渐变暗，小手消失，光敏开关闭合，灯泡亮，电流走起
				GetComponent<PhotoRecognizingPanel> ().ShowFinger (transform.Find ("SunAndMoonWidget").localPosition);
				isFingerShow = true;
			}
			if (!transform.Find ("SunAndMoonWidget").GetComponent<MoonAndSunCtrl> ().isDaytime) {//如果是晚上
				if (!isFingerDestroyed) {
					Destroy (PhotoRecognizingPanel._instance.finger);	
					isFingerDestroyed = true;
				}
				changeTimer += Time.deltaTime;
				if (changeTimer >= changeTime) {
					changeTimer = changeTime;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);//蒙版渐变暗
				if (changeTimer >= changeTime * 5 / 6) {//背景渐变到一半的时候
					isCircuitWork = true;
				}
			} else { //如果是白天
				changeTimer -= Time.deltaTime;
				if (changeTimer <= 0) {
					changeTimer = 0;
				}
				nightBg.alpha = Mathf.Lerp (0, 1f, changeTimer / changeTime);
				if (changeTimer <= changeTime / 6) {
					isCircuitWork = false;
				}
			}	

			CurrentFlow._instance.switchOnOff (int.Parse (LAswitch.gameObject.tag), isCircuitWork);
			CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);	
			LAswitch.GetComponent<UISprite> ().spriteName = (isCircuitWork ? "LAswitchOn" : "LAswitchOff");
		}
	}


}
