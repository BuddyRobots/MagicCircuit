using UnityEngine;
using System.Collections;



//共用面板作为数据传递的桥梁，数据共享，非常重要  levelitemui --->sendmsg --> CommonPanelCtrl --> DescriptionPanel--show
public class CommonPanel01: MonoBehaviour {
	
	public static CommonPanel01 _instance;

	private GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject startPanel;

	private LevelUI levelSelectPanel;
	private DescriptionPanel levelDescriptionPanel;

	//public bool isLevelSPanel;//是否在选关界面
	//public bool isLevelDPanel;//是否在描述界面
	//public static int panelFlag;//定一个标志来判断是在选关界面还是描述界面,1--选关，2---描述

	void Awake()
	{
		_instance = this;
	}

	void Start () 
	{
		

		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicBtn=transform.Find("MusicBtn").GetComponent<UIButton> ().gameObject;
		startPanel=transform.parent.Find ("StartPanel").gameObject;

		levelSelectPanel = transform.Find ("LevelSelectPanel").GetComponent<LevelUI> ();
		levelDescriptionPanel = transform.Find("DescriptionPanel").GetComponent<DescriptionPanel>();

		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get (musicBtn).onClick = OnMusicBtnClick;


	}

	void Update()
	{
		


	}

	//需要接收从levelSelectPanel传递过来的等级数字来传递给levelDescriptionPanel显示什么样的描述
	public void OnLevelItemClick(object[] objectArray)
	{

		Debug.Log ("onLevelItemClick");
		LevelItemData data = objectArray[0] as LevelItemData;
		//取出data里面的数据,调用DescriptionPanel的show方法
		Debug.Log(data);
		if(levelDescriptionPanel == null){
			levelDescriptionPanel = transform.Find("DescriptionPanel").GetComponent<DescriptionPanel>();
		}
		levelDescriptionPanel.Show(data);
	}

	public void PanelOn()
	{
		gameObject.SetActive (true);
	}
	public void PanelOff()
	{
		levelSelectPanel.gameObject.SetActive (false);//隐藏选关界面
		levelDescriptionPanel.gameObject.SetActive (false);//隐藏描述界面
		gameObject.SetActive (false);
	}

	void OnHomeBtnClick(GameObject btn)
	{
		Debug.Log ("OnHomeBtnClick");
		//关闭当前界面
		PanelOff ();
		//返回主界面  to do...
		startPanel.SetActive (true);//for test...

//		if (panelFlag == 1) 
//		{
//			//在选关界面
//
//
//
//		} 
//		else if (panelFlag == 2) 
//		{
//			//在描述界面
//			levelDescriptionPanel.gameObject.SetActive(false);
//
//		}


	}

	void OnMusicBtnClick(GameObject btn)
	{
		Debug.Log ("musicBtn is clicked");
		//关闭当前界面
		//PanelOff ();
		//返回声音设置界面  to do...

	}



}
