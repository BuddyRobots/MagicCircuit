using UnityEngine;
using System.Collections;

public class CommonPanel02 : MonoBehaviour {

	public static CommonPanel02 _instance;

	private GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject helpBtn;


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
		//返回声音界面
		Debug.Log("GameObject: "+btn.name);

	}

	void OnHelpBtnClick(GameObject btn)
	{
		//返回帮助界面
		Debug.Log("GameObject: "+btn.name);
	}
}
