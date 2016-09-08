using UnityEngine;
using System.Collections;

public class MicroPhoneBtnCtrl : MonoBehaviour {

	[HideInInspector]
	public bool isCollectVoice = false;

	private int clickCount = 0;

	void OnEnable () 
	{

		clickCount = 0;
		isCollectVoice = false;
	
	}
	

	void Update () {
	
	}




	void OnClick()
	{
		clickCount = 1;
		//if (clickCount == 1) 
		//{
			isCollectVoice = true;
		//}
	}
}
