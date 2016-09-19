using UnityEngine;
using System.Collections;
//level 1
public class PlayCircuitAnimation : MonoBehaviour 
{

	/// <summary>
	///执行该脚本中update函数中的方法的标志
	/// </summary>
	[HideInInspector]
	public bool isPlayCircuitAnimation=false;


	void Update () 
	{
		
		if (isPlayCircuitAnimation)
		{
			if (!GetComponent<PhotoRecognizingPanel> ().isArrowShowDone)//如果没有播放过电流
			{
				CommonFuncManager._instance.OpenCircuit ();
				//OpenCircuit ();
			}
		}
	}

	/// <summary>
	/// 接通电路
	/// </summary>
//	public void OpenCircuit()
//	{
//		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";//灯亮 
//		GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);//走电流
//		GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;//标记已经播放电流
//	}
}
