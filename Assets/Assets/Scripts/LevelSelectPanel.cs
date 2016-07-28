using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class LevelSelectPanel : MonoBehaviour {


	//用来控制关卡列表，
	//包含很多个LevelItemUI---用列表来存储

	public static LevelSelectPanel _instance;

	private GameObject startPanel;
	private GameObject levelDescriptionPanel;

	private GameObject homeBtn;
	private GameObject musicOnBtn;
	private GameObject musicOffBtn;

	private bool isMusicOn;

	private UIButton btnT01;
	private UIButton btnT03;
	private UIButton btnL01;
	private UIButton btnL03;

	public List<UIButton> uiBtnListT = new List<UIButton>();
	public List<UIButton> uiBtnListL = new List<UIButton>();

	void Awake()
	{
		_instance = this;


	}

	void Start()
	{

		Debug.Log ("AAAAAAAAA");
		startPanel = transform.parent.Find ("StartPanel").gameObject;
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;
		btnT01 = transform.Find ("Bg/LevelD01/LevelT01").GetComponent<UIButton> ();
		btnL01 = transform.Find ("Bg/LevelD01/LevelL01").GetComponent<UIButton> ();

		btnT03 = transform.Find ("Bg/LevelD03/LevelT03").GetComponent<UIButton> ();
		btnL03 = transform.Find ("Bg/LevelD03/LevelL03").GetComponent<UIButton> ();

		uiBtnListT.Add (btnT01);
		uiBtnListT.Add (btnT03);

		uiBtnListL.Add (btnL01);
		uiBtnListL.Add (btnL03);

		Debug.Log ("BBBBB");



		UIEventListener.Get(homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
		Debug.Log ("CCCCCC");
		refreshLevelUI ();

	}



	public void PanelOff()
	{

		gameObject.SetActive (false);

	}

	void OnHomeBtnClick(GameObject btn)
	{
		Debug.Log ("btn: " + btn.name + " clicked");
		//关闭当前界面
		PanelOff ();
		//跳到主界面
		startPanel.SetActive (true);

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

	void Update()
	{
		//print (musicBtn.GetComponent<UISprite> ().spriteName);
	}


	//---------在levelItemUI中实现点击
	/// <summary>
	/// 点击关卡按钮触发的操作
	/// </summary>
	/// <param name="btn">传递的参数是点击的按钮对象</param>
//	public void OnLevelBtnClick(GameObject btn)
//	{
//
//		Debug.Log ("levelBtn: "+btn.name+" is clicked");
//		//关闭当前界面，显示关卡描述界面
//		gameObject.SetActive (false);
//		levelDescriptionPanel.SetActive (true);
//
//		//得到关卡数字
//		int levelID = GetLevel (btn.name);
//
//		LevelItemData itemData = LevelManager._instance.GetSingleLevelItem (levelID);
//		if(itemData!=null)
//		{
//			object[] objectArray = new object[2];
//			objectArray[0] = itemData;//这里可以把整个item的数据都传过去，方便后面使用，不要只传等级数字
//			objectArray[1] = this;
//
//			//transform.parent.parent.parent.parent.SendMessage("OnLevelItemClick", objectArray);
//			DescriptionPanel._instance.SendMessage("Show",itemData);
//		}
//
//
//	}

	/// <summary>
	/// 获取关卡
	/// </summary>
	/// <returns>返回关卡数字</returns>
	/// <param name="levelName">参数是“Dark01”类型的字符串</param>
	/// 如果返回0则表示解析错误；
	public int GetLevel(string levelName)
	{
		int ret = 0;

		string temp =levelName.Substring (4);
		if (temp [0] == 0)
		{

			string temp2 = temp.Substring (1);
			int.TryParse (temp2, out ret);
		} 
		else
		{
			if (int.TryParse (temp, out ret))
			{


			}
		}

		return ret;
	}


//	//刷新UI
	public void refreshLevelUI()
	{
		
//		for(int i=0; i < LevelManager._instance.levelItemDataList.Count; ++i)
		//{
//			UIButton btnT = uiBtnListT [i];
//			UIButton btnL = uiBtnListL [i];
//			string levelId = btnT.substirng ();
//
//			if(levelId == LevelManager._instance.levelItemDataList[i].LevelID)
		//{
//				switch(data.Progress)
//				{
//				case LevelProgress.Todo:
//					Debug.Log ("refreshLevelUI122");
//					btnT.gameObject.SetActive (false);
//					btnL.gameObject.SetActive (false);
//					break;
//				case LevelProgress.Doing:
//					Debug.Log ("refreshLevelUI13333");
//					btnL.gameObject.SetActive (true);
//					btnT.gameObject.SetActive (false);
//
//					break;
//				case LevelProgress.Done:
//					Debug.Log ("refreshLevelUI4444");
//					btnL.gameObject.SetActive (false);
//					btnT.gameObject.SetActive (true);
//					break;
//				default:
//					break;
//
//
//				}
//				break;
//			}
			

		
		//Debug.Log ("refreshLevelUI_AAA");
		//Debug.Log ("LevelManager._instance.levelItemDataList==size" + LevelManager._instance.levelItemDataList.Count);

		foreach(LevelItemData data in LevelManager._instance.levelItemDataList)
		{
			//Debug.Log ("LevelManager._instance.levelItemDataList==size" + LevelManager._instance.levelItemDataList.Count);
			//LevelItemData data = LevelManager._instance.levelItemDataList[i];

			switch(data.LevelID)
			{
			case 1:
				//Debug.Log ("1data.Progress" + data.Progress);
				switch(data.Progress)
				{
				case LevelProgress.Todo:
					//Debug.Log ("refreshLevelUI122");
				btnT01.gameObject.SetActive(false);
				btnL01.gameObject.SetActive (false);
					break;
				case LevelProgress.Doing:
					//Debug.Log ("refreshLevelUI13333");
				btnT01.gameObject.SetActive (false);
				btnL01.gameObject.SetActive (false);

					break;
				case LevelProgress.Done:
					//Debug.Log ("refreshLevelUI4444");
				
				btnT01.gameObject.SetActive (false);
				btnL01.gameObject.SetActive(false);
					break;
				default:
					break;


				}
				break;
			case 3:
				//Debug.Log ("3data.Progress" + data.Progress);
				switch(data.Progress)
				{
				case LevelProgress.Todo:
					btnT03.gameObject.SetActive (false);
					btnL03.gameObject.SetActive (false);

					break;
				case LevelProgress.Doing:
					btnT03.gameObject.SetActive (true);
					btnL03.gameObject.SetActive (false);
					break;
				case LevelProgress.Done:
					btnT03.gameObject.SetActive (false);
					btnL03.gameObject.SetActive (true);
					break;
				default:
					break;


				}

				break;
			default:
				break;


			}
			

		}


	}



}
