using UnityEngine;
using System.Collections;


//level 11
public class VOswitchOccur : MonoBehaviour 
{
	[HideInInspector]
	public bool isVOswitchOccur=false;

	//const int SOUND_CRITERION = 1;//音量大小标准，可以调整以满足具体需求


	void OnEnable () 
	{
		isVOswitchOccur=false;
	
	}
	
	private bool isStartRecord = false;
	void Update () 
	{
		if (isVOswitchOccur) 
		{
			if (!GetComponent<PhotoRecognizingPanel>().isArrowShowDone) 
			{
				//在话筒按钮出现小手
				GetComponent<PhotoRecognizingPanel> ().ShowFinger(transform.Find("MicroPhoneBtn").localPosition);
				//点击话筒按钮，
				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
				{

					Destroy (PhotoRecognizingPanel._instance.finger);
					//弹出提示框，
					PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);
					if (!isStartRecord) 
					{
						//收集声音  to do...
						MicroPhoneInput.getInstance().StartRecord();
						isStartRecord = true;
					}
						
					//收集到声音后，播放声音收集完成音效，提示框消失， to do...
					//if(MicroPhoneInput.getInstance().isSoundLoudEnough())
					if(CommonFuncManager._instance.isSoundLoudEnough())
					{
						MicroPhoneInput.getInstance().StopRecord();
						PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
						transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
						GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
					}


				}

			}

		}

	}
		




//	public bool isSoundLoudEnough()
//	{
//		float volume = MicroPhoneInput.getInstance ().getSoundVolume();
//		if(volume > SOUND_CRITERION)
//		{
//			return true;
//		}
//
//		return false;
//	}


}
