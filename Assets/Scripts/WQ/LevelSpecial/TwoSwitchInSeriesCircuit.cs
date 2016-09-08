using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoSwitchInSeriesCircuit : MonoBehaviour {

	private List<GameObject> normalSwitchList =null;
	private int animationPlayedTimes=0;

	[HideInInspector]
	public bool isTwoSwitchInSeriesCircuit = false;

	void OnEnable () 
	{
		animationPlayedTimes=0;
		isTwoSwitchInSeriesCircuit = false;
	}


	void Update () 
	{
		if (isTwoSwitchInSeriesCircuit) 
		{
			normalSwitchList = GetComponent<PhotoRecognizingPanel> ().switchList;
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone) 
			{
				
				for (int i = 0; i <normalSwitchList .Count; i++) 
				{
					if (normalSwitchList[i].GetComponent<SwitchCtrl>().isSwitchOn==false && i==normalSwitchList.Count-1)//如果串联电路上的开关都闭合了
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
			}

			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{
				for (int i = 0; i < normalSwitchList.Count; i++) 
				{
					if (normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn )//只要有一个开关断开，就停止电流 
					{
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbDark";
						if (transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
						{
							transform.Find ("loudspeaker").GetComponent<AudioSource> ().Pause ();
						}
						foreach (GameObject item in GetComponent<PhotoRecognizingPanel> ().arrowList) 
						{
							//电流应该隐藏而不是销毁    to do ..
							//item.SetActive(false);
							Destroy (item);
						}
					
					}
					else if(normalSwitchList[i].GetComponent<SwitchCtrl>().isSwitchOn==false && i==normalSwitchList.Count-1)
					{
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";
						if (!transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
						{
							transform.Find ("loudspeaker").GetComponent<AudioSource> ().Play ();
						}
					}
				}
			
			}
			
		}
	
	}
}
