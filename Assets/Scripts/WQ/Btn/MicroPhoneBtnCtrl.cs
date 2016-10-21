using UnityEngine;
using System.Collections;

public class MicroPhoneBtnCtrl : MonoBehaviour {

	[HideInInspector]
	public bool isCollectVoice = false;


	void OnEnable () 
	{

		isCollectVoice = false;
	
	}

	void Update () 
	{
	
	}

	void OnClick()
	{
		//只有光敏开关闭合的时候，点击小话筒按钮才会出现提示框，开始收集声音
		//如果光敏开关没有闭合，点击小话筒按钮，没有反应


			isCollectVoice = true;

	}
}
