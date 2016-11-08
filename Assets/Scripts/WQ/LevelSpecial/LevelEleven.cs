using UnityEngine;
using System.Collections;
using MagicCircuit;

//level 11
public class LevelEleven : MonoBehaviour 
{
	[HideInInspector]
	public bool isVOswitchOccur=false;
	private bool isAnimationPlay=false;
	private bool isStartRecord = false;

	private Transform voiceSwitch;

	/// <summary>
	/// 保证声音收集一次的标志
	/// </summary>
	private bool stayForAwhile = false;

	//const int SOUND_CRITERION = 1;//音量大小标准，可以调整以满足具体需求

	void OnEnable () 
	{
		isVOswitchOccur=false;
		isAnimationPlay=false;
		isStartRecord = false;
		stayForAwhile = false;

		voiceSwitch=transform.Find("voiceOperSwitch");
	
	}


	void Update () 
	{
		if (isVOswitchOccur) 
		{
//				Transform voiceSwitch=transform.Find("voiceOperSwitch");
				//在话筒按钮出现小手
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);
				//点击话筒按钮，
				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
				{
					if (PhotoRecognizingPanel._instance.finger) 
					{
						Destroy (PhotoRecognizingPanel._instance.finger);
						
					}
					if (!isStartRecord) 
					{  
						PhotoRecognizingPanel._instance.noticeToMakeVoice.SetActive(true);//弹出提示框
						PhotoRecognizingPanel._instance.voiceCollectionMark.SetActive(true);//弹出声音收集图片
						PhotoRecognizingPanel._instance.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = true;//显示声音收集动画
						MicroPhoneInput.getInstance().StartRecord();//收集声音
						isStartRecord = true;
					}
					//收集到声音后，播放声音收集完成音效，提示框消失
					if (CommonFuncManager._instance.isSoundLoudEnough ()) 
					{
						isAnimationPlay = true;
						PhotoRecognizingPanel._instance.noticeToMakeVoice.SetActive (false);
						PhotoRecognizingPanel._instance.voiceCollectionMark.transform.Find ("Wave").GetComponent<MyAnimation> ().canPlay = false;
						PhotoRecognizingPanel._instance.voiceCollectionMark.SetActive (false);
						MicroPhoneInput.getInstance ().StopRecord ();
						GetImage._instance.cf.switchOnOff (int.Parse (voiceSwitch.gameObject.tag), true);
						voiceSwitch.GetComponent<UISprite>().spriteName="VOswitchOn";
						CommonFuncManager._instance.CircuitItemRefresh (GetImage._instance.itemList);	
					} 

					CommonFuncManager._instance.ArrowsRefresh(GetImage._instance.itemList);
				}	
		}
	}



}
