using UnityEngine;
using System.Collections;

public class CommonPanel02 : MonoBehaviour {

	public static CommonPanel02 _instance;

	private GameObject homeBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;
	private GameObject helpBtn;
	private bool isMusicOn = true;

	void Awake()
	{
		_instance = this;

		homeBtn = transform.Find ("HomeBtn").gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;
		helpBtn = transform.Find ("HelpBtn").gameObject;

		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
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

	void OnMusicOnBtnClick(GameObject btn)
	{
		isMusicOn=false;
		//musicBtn.GetComponent<UISprite> ().spriteName = (isMusicOn ? "音乐" : "静音");
		musicOffBtn.SetActive (true);
		musicOnBtn.SetActive (false);

		//声音开关  to do ....
	}
	void  OnMusicOffBtnClick(GameObject btn)  
	{
		isMusicOn = true;
		musicOffBtn.SetActive (false);
		musicOnBtn.SetActive (true);

	}

	void OnHelpBtnClick(GameObject btn)
	{
		//跳转到帮助界面
		Debug.Log("GameObject: "+btn.name);
	}
}
