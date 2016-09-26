using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MagicCircuit;
//第4关
public class LoudSpeakerInLevelFour : MonoBehaviour 
{

	[HideInInspector]
	public bool isLoudSpeakerOccur = false;
	private int animationPlayedTimes=0;
	private List<GameObject> normalSwitchList =null;

	void OnEnable()
	{
		animationPlayedTimes=0;
		isLoudSpeakerOccur = false;
	}
		
	void Update () 
	{
		if(isLoudSpeakerOccur)
		{

			normalSwitchList = GetComponent<PhotoRecognizingPanel> ().switchList;

			for (int i = 0; i < normalSwitchList.Count; i++) 
			{
				CurrentFlow._instance.switchOnOff (int.Parse (normalSwitchList [i].tag), normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn ? false : true);

				CommonFuncManager._instance.CircuitReset (CurrentFlow._instance.circuitItems);

			}


//			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
//			{
//				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
//				{
//					//喇叭响  to do...
//					transform.Find("loudspeaker").GetComponent<AudioSource>().Play();
//					CommonFuncManager._instance.OpenCircuit ();
//					animationPlayedTimes=1;//电流播放一次
//				}	
//			}
//			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
//			{
//				CommonFuncManager._instance.CircuitOnOrOff (!transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn);
//				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn) 
//				{
//					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
//					//喇叭停止播放声音  to do ...
//					if (transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
//					{
//						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Pause ();
//					}
//
//				}
//				else 
//				{
//					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
//					if (!transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
//					{
//						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Play ();
//					}
//				}
//
//			}

		}



	}
}
