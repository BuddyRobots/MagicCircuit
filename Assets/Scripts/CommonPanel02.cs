using UnityEngine;
using System.Collections;

public class CommonPanel02 : MonoBehaviour {

	public static CommonPanel02 _instance;

	private GameObject helpBtn;
	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;


		helpBtn = transform.Find ("HelpBtn").gameObject;
		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;

	}
		

	void OnHelpBtnClick(GameObject btn)
	{
		//跳转到帮助界面
		Debug.Log("GameObject: "+btn.name);
	}
}
