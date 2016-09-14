using UnityEngine;
using System.Collections;

//第4关
public class LoudSpeakerInLevelFour : MonoBehaviour {

	[HideInInspector]
	public bool isLoudSpeakerOccur = false;
	private int animationPlayedTimes=0;

	void OnEnable()
	{
		animationPlayedTimes=0;
		isLoudSpeakerOccur = false;
	}
		
	void Update () 
	{
		if(isLoudSpeakerOccur)
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
				{
					//喇叭响  to do...
					transform.Find("loudspeaker").GetComponent<AudioSource>().Play();
					CommonFuncManager._instance.OpenCircuit ();
					animationPlayedTimes=1;//电流播放一次
				}	
			}
			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				CommonFuncManager._instance.CircuitOnOrOff (!transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn);
				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn) 
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
					//喇叭停止播放声音  to do ...
					if (transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
					{
						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Pause ();
					}

				}
				else 
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
					if (!transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
					{
						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Play ();
					}
				}

			}

		}



	}
}
