﻿using UnityEngine;
using System.Collections;

public class VOswitchOccur : MonoBehaviour {



	[HideInInspector]
	public bool isVOswitchOccur=false;


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
				GetComponent<PhotoRecognizingPanel> ().ShowFingerOnLine(transform.Find("MicroPhoneBtn").localPosition);
				//点击话筒按钮，
				if (transform.Find ("MicroPhoneBtn").GetComponent<MicroPhoneBtnCtrl> ().isCollectVoice) 
				{

					Destroy (PhotoRecognizingPanel._instance.finger);
					//弹出提示框，
					PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(true);
					if (!isStartRecord) 
					{
						//收集声音  to do...
						MicrophoneManager._instance.StartRecord();
						isStartRecord = true;
					}

					if (MicrophoneManager._instance.isVoiceCollected) 
					{
						PhotoRecognizingPanel._instance.voiceNoticeBg.SetActive(false);
						transform.Find("voiceOperSwitch").GetComponent<UISprite>().spriteName="VOswitchOn";
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
						GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
						GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;

					}


					//收集到声音后，播放声音收集完成音效，提示框消失， to do...

				}

			}

		}

	}
}
