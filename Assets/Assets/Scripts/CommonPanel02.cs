using UnityEngine;
using System.Collections;

public class CommonPanel02 : MonoBehaviour {

	public static CommonPanel02 _instance;

	private GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject helpBtn;
	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;

		homeBtn = transform.Find ("HomeBtn").gameObject;
		musicBtn = transform.Find ("MusicBtn").gameObject;
		helpBtn = transform.Find ("HelpBtn").gameObject;

		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get (musicBtn).onClick = OnMusicBtnClick;
		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;

	}



	void Start () 
	{
	
	}
	

	void Update () 
	{
	
	}

	void OnHomeBtnClick(GameObject btn)
	{
		//关闭当前界面，返回主界面
		Debug.Log("GameObject: "+btn.name);

		gameObject.SetActive (false);
		transform.parent.Find ("StartPanel").gameObject.SetActive (true);

	}

	void OnMusicBtnClick(GameObject btn)
	{
		//图标变换,
		isMusicOn=!isMusicOn;

		musicBtn.GetComponent<UISprite> ().spriteName = (isMusicOn ? "音乐" : "静音");
		//声音开关  to do ....

	}

	void OnHelpBtnClick(GameObject btn)
	{
		//返回帮助界面
		Debug.Log("GameObject: "+btn.name);
	}
}
