using UnityEngine;
using System.Collections;

public class LoudSpeakerInLevelFour : MonoBehaviour {

	[HideInInspector]
	public bool isLoudSpeakerOccur = false;
	private int animationPlayedTimes=0;
	void OnEnable()
	{
		animationPlayedTimes=0;
		isLoudSpeakerOccur = false;
	}

	void Start () {
	
	}
	

	void Update () 
	{
	
		if(isLoudSpeakerOccur)
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{

				if(transform.Find("switch").GetComponent<SwitchCtrl>().isSwitchOn==false)//开关闭合
				{
					//灯泡变亮
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";
					//喇叭响  to do...
					transform.Find("loudspeaker").GetComponent<AudioSource>().Play();
					//走电流 
					GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);
					GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;
					animationPlayedTimes=1;//电流播放一次
				}	
			
			
			}
			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				
				if (transform.Find ("switch").GetComponent<SwitchCtrl> ().isSwitchOn) {
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbDark";
					//喇叭停止播放声音  to do ...
					if (transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) {
					
						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Pause ();
					}
					//GetComponent<PhotoRecognizingPanel> ().StopCreateArrows();
					foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList) {
						//电流应该隐藏而不是销毁    to do ..
						//item.SetActive(false);
						Destroy (item);
					}

				}
				else 
				{
					transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";
					if (!transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) {
						transform.Find ("loudspeaker").GetComponent<AudioSource> ().Play ();
					}
				}

			}

		}



	}
}
