using UnityEngine;
using System.Collections;
using MagicCircuit;

//level 11
public class VOswitchOccur : MonoBehaviour 
{
	[HideInInspector]
	public bool isVOswitchOccur=false;
	private bool isAnimationPlay=false;
	private bool isStartRecord = false;

	//const int SOUND_CRITERION = 1;//音量大小标准，可以调整以满足具体需求


	void OnEnable () 
	{
		isVOswitchOccur=false;
		isAnimationPlay=false;
		isStartRecord = false;
	
	}

	void Update () 
	{
		if (isVOswitchOccur) 
		{
				Transform voiceSwitch=transform.Find("voiceOperSwitch");
				//在话筒按钮出现小手
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);
				//点击话筒按钮，
				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
				{
					Destroy (PhotoRecognizingPanel._instance.finger);
					PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);//弹出提示框，
					if (!isStartRecord) 
					{  
						MicroPhoneInput.getInstance().StartRecord();//收集声音
						isStartRecord = true;
					}
					//收集到声音后，播放声音收集完成音效，提示框消失
					if (CommonFuncManager._instance.isSoundLoudEnough ()) 
					{
						MicroPhoneInput.getInstance ().StopRecord ();
						PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive (false);
						isAnimationPlay = true;
					} 
					if (isAnimationPlay) 
					{
						CurrentFlow._instance.switchOnOff (int.Parse (voiceSwitch.gameObject.tag), true);
						CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);	
					}
				}	
		}
	}

}
