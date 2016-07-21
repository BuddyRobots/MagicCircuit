using UnityEngine;
using System.Collections;

public class StartPanel : MonoBehaviour {
	
	public static StartPanel _instance;
	private  GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject nextBtn;
	private GameObject commonPannel01;
	private GameObject levelSelectPanel;
	private GameObject levelDescriptionPanel;


	void Awake()
	{

		_instance = this;

	}

	void Start () 
	{		

		homeBtn = transform.Find ("HomeBtn").gameObject;
		musicBtn= transform.Find ("MusicBtn").gameObject;
		nextBtn= transform.Find ("NextBtn").gameObject;

		commonPannel01=transform.parent.Find("CommonPanel01").gameObject;
		levelSelectPanel=transform.parent.Find("CommonPanel01/LevelSelectPanel").gameObject;
		levelDescriptionPanel=transform.parent.Find("CommonPanel01/DescriptionPanel").gameObject;

		UIEventListener.Get(homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicBtn).onClick = OnMusicBtnClick;
		UIEventListener.Get(nextBtn).onClick = OnNextBtnClick;

		if(commonPannel01==null)
		{
			Debug.Log ("commonPannel01 is null");

		}
	}
	

	void Update () {
	
	}

	//进入主界面
	void OnHomeBtnClick(GameObject btn)
	{


		Debug.Log ("HomeBtn clicked");
		//return to homePage  
		//to do...


		//PanelOff ();

	}


	//声音设置
	void OnMusicBtnClick(GameObject btn)
	{
		Debug.Log ("musicBtn clicked");
		//MusicSet panel pop up 
		//to do...

		//PanelOff ();
	}

	void OnNextBtnClick(GameObject btn)
	{
		Debug.Log ("NextBtn Clicked");
		//关闭当前界面，切换到关卡选择界面
		PanelOff ();


		//显示选关界面
		commonPannel01.SetActive(true);
		levelSelectPanel.SetActive (true);
		levelDescriptionPanel.SetActive (false);

		//CommonPanel01._instance.PanelOn ();
		//LevelUI._instance.PanelOn ();
		//DescriptionPanel._instance.PanelOff ();
		//CommonPanel01.panelFlag=1;
	}

	public void PanelOff()
	{
		gameObject.SetActive (false);

	}

	public void PanelOn()
	{

		gameObject.SetActive (true);
	}




}
