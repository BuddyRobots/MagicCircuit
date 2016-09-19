using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//level 5
public class TwoSwitchInSeriesCircuit : MonoBehaviour 
{
	private List<GameObject> normalSwitchList =null;
	private int animationPlayedTimes=0;

	[HideInInspector]
	public bool isTwoSwitchInSeriesCircuit = false;

	/// <summary>
	/// 电路是否接通的标志
	/// </summary>
	private bool isCircuitOpen = false;

	void OnEnable () 
	{
		animationPlayedTimes=0;
		isTwoSwitchInSeriesCircuit = false;
		isCircuitOpen = false;
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
					if (!normalSwitchList[i].GetComponent<SwitchCtrl>().isSwitchOn && i==normalSwitchList.Count-1)//如果串联电路上的开关都闭合了
					{
						//喇叭响  to do...
						transform.Find("loudspeaker").GetComponent<AudioSource>().Play();
						CommonFuncManager._instance.OpenCircuit ();
						animationPlayedTimes=1;//电流播放一次
					}
				}
			}

			if (animationPlayedTimes==1) //如果在播放电流的时候点击开关断开
			{

				CommonFuncManager._instance.CircuitOnOrOff (isCircuitOpen);
				for (int i = 0; i < normalSwitchList.Count; i++) 
				{
					if (normalSwitchList [i].GetComponent<SwitchCtrl> ().isSwitchOn )//只要有一个开关断开，就停止电流 
					{
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOff";
						if (transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
						{
							transform.Find ("loudspeaker").GetComponent<AudioSource> ().Pause ();
						}
						isCircuitOpen=false;
					
					}
					else if(!normalSwitchList[i].GetComponent<SwitchCtrl>().isSwitchOn && i==normalSwitchList.Count-1)//如果两个都是闭合的
					{
						transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";
						if (!transform.Find ("loudspeaker").GetComponent<AudioSource> ().isPlaying) 
						{
							transform.Find ("loudspeaker").GetComponent<AudioSource> ().Play ();
						}
						isCircuitOpen = true;
					}
				}
			
			}
			
		}
	
	}
}
