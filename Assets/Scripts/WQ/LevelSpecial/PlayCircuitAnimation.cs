using UnityEngine;
using System.Collections;

public class PlayCircuitAnimation : MonoBehaviour {

	/// <summary>
	///执行该脚本中update函数中的方法的标志
	/// </summary>
	[HideInInspector]
	public bool isPlayCircuitAnimation=false;

	void Update () 
	{
		
		if (isPlayCircuitAnimation)
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone)//如果没有播放电流就播放电流
			{
				//以下只针对在没有开关的情况下，直接点亮灯泡走电流
				transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulb-on";//灯亮 
				GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);//走电流
				GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;//标记已经播放电流
			}
		}
	}
}
