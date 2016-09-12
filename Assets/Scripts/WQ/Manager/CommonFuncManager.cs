using UnityEngine;
using System.Collections;

public class CommonFuncManager : MonoBehaviour {

	public static CommonFuncManager _instance;
	private bool tempIsLaSwitch = false;

	void Awake()
	{
		_instance = this;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/// <summary>
	/// 接通电路
	/// </summary>
	public void OpenCircuit()
	{
		transform.Find ("bulb").GetComponent<UISprite> ().spriteName = "bulbOn";//灯亮 
		GetComponent<PhotoRecognizingPanel> ().ArrowShowLineByLine(PhotoRecognizingPanel._instance.lines,0);//走电流
		GetComponent<PhotoRecognizingPanel> ().isArrowShowDone = true;//标记已经播放电流
	}



	/// <summary>
	/// 电流动画的开启和关闭
	/// </summary>
	/// <param name="isSwitch">If set to <c>true</c> is switch.</param>
	public  void CircuitOnOrOff(bool isSwitch)
	{
		if (tempIsLaSwitch != isSwitch) 
		{
			if (!isSwitch) 
			{
				GetComponent<PhotoRecognizingPanel> ().StopCircuit ();
			}
			else
			{
				GetComponent<PhotoRecognizingPanel> ().ContinueCircuit ();
			}
			tempIsLaSwitch = isSwitch;
		} 
	}
}
