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
	private UIButton btnT02;
	private UIButton btnT03;
	private UIButton btnT04;
	private UIButton btnT05;
	private UIButton btnT06;
	private UIButton btnT07;
	private UIButton btnT08;
	private UIButton btnT09;
	private UIButton btnT10;
	private UIButton btnT11;
	private UIButton btnT12;
	private UIButton btnT13;
	private UIButton btnT14;
	private UIButton btnT15;

	private UIButton btnL01;
	private UIButton btnL02;
	private UIButton btnL03;
	private UIButton btnL04;
	private UIButton btnL05;
	private UIButton btnL06;
	private UIButton btnL07;
	private UIButton btnL08;
	private UIButton btnL09;
	private UIButton btnL10;
	private UIButton btnL11;
	private UIButton btnL12;
	private UIButton btnL13;
	private UIButton btnL14;
	private UIButton btnL15;


	public List<UIButton> uiBtnListT = new List<UIButton>();
	public List<UIButton> uiBtnListL = new List<UIButton>();

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		startPanel = transform.parent.Find ("StartPanel").gameObject;
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicOnBtn = transform.Find ("MusicOnBtn").GetComponent<UIButton> ().gameObject;
		musicOffBtn = transform.Find ("MusicOffBtn").GetComponent<UIButton> ().gameObject;

		btnT01 = transform.Find ("Bg/LevelD01/LevelT01").GetComponent<UIButton> ();
		btnL01 = transform.Find ("Bg/LevelD01/LevelL01").GetComponent<UIButton> ();

		btnT02 = transform.Find ("Bg/LevelD02/LevelT02").GetComponent<UIButton> ();
		btnL02 = transform.Find ("Bg/LevelD02/LevelL02").GetComponent<UIButton> ();

		btnT03 = transform.Find ("Bg/LevelD03/LevelT03").GetComponent<UIButton> ();
		btnL03 = transform.Find ("Bg/LevelD03/LevelL03").GetComponent<UIButton> ();

		btnT04 = transform.Find ("Bg/LevelD04/LevelT04").GetComponent<UIButton> ();
		btnL04 = transform.Find ("Bg/LevelD04/LevelL04").GetComponent<UIButton> ();

		btnT05 = transform.Find ("Bg/LevelD05/LevelT05").GetComponent<UIButton> ();
		btnL05 = transform.Find ("Bg/LevelD05/LevelL05").GetComponent<UIButton> ();

		btnT06 = transform.Find ("Bg/LevelD06/LevelT06").GetComponent<UIButton> ();
		btnL06 = transform.Find ("Bg/LevelD06/LevelL06").GetComponent<UIButton> ();

		btnT07 = transform.Find ("Bg/LevelD07/LevelT07").GetComponent<UIButton> ();
		btnL07 = transform.Find ("Bg/LevelD07/LevelL07").GetComponent<UIButton> ();

		btnT08 = transform.Find ("Bg/LevelD08/LevelT08").GetComponent<UIButton> ();
		btnL08 = transform.Find ("Bg/LevelD08/LevelL08").GetComponent<UIButton> ();

		btnT09 = transform.Find ("Bg/LevelD09/LevelT09").GetComponent<UIButton> ();
		btnL09 = transform.Find ("Bg/LevelD09/LevelL09").GetComponent<UIButton> ();

		btnT10 = transform.Find ("Bg/LevelD10/LevelT10").GetComponent<UIButton> ();
		btnL10 = transform.Find ("Bg/LevelD10/LevelL10").GetComponent<UIButton> ();

		btnT11 = transform.Find ("Bg/LevelD11/LevelT11").GetComponent<UIButton> ();
		btnL11 = transform.Find ("Bg/LevelD11/LevelL11").GetComponent<UIButton> ();

		btnT12 = transform.Find ("Bg/LevelD12/LevelT12").GetComponent<UIButton> ();
		btnL12 = transform.Find ("Bg/LevelD12/LevelL12").GetComponent<UIButton> ();

		btnT13 = transform.Find ("Bg/LevelD13/LevelT13").GetComponent<UIButton> ();
		btnL13 = transform.Find ("Bg/LevelD13/LevelL13").GetComponent<UIButton> ();

		btnT14 = transform.Find ("Bg/LevelD14/LevelT14").GetComponent<UIButton> ();
		btnL14 = transform.Find ("Bg/LevelD14/LevelL14").GetComponent<UIButton> ();

		btnT15 = transform.Find ("Bg/LevelD15/LevelT15").GetComponent<UIButton> ();
		btnL15 = transform.Find ("Bg/LevelD15/LevelL15").GetComponent<UIButton> ();



		uiBtnListT.Add (btnT01);
		uiBtnListT.Add (btnT02);
		uiBtnListT.Add (btnT03);
		uiBtnListT.Add (btnT04);
		uiBtnListT.Add (btnT05);
		uiBtnListT.Add (btnT06);
		uiBtnListT.Add (btnT07);
		uiBtnListT.Add (btnT08);
		uiBtnListT.Add (btnT09);
		uiBtnListT.Add (btnT10);
		uiBtnListT.Add (btnT11);
		uiBtnListT.Add (btnT12);
		uiBtnListT.Add (btnT13);
		uiBtnListT.Add (btnT14);
		uiBtnListT.Add (btnT15);

		uiBtnListL.Add (btnL01);
		uiBtnListL.Add (btnL02);
		uiBtnListL.Add (btnL03);
		uiBtnListL.Add (btnL04);
		uiBtnListL.Add (btnL05);
		uiBtnListL.Add (btnL06);
		uiBtnListL.Add (btnL07);
		uiBtnListL.Add (btnL08);
		uiBtnListL.Add (btnL09);
		uiBtnListL.Add (btnL10);
		uiBtnListL.Add (btnL11);
		uiBtnListL.Add (btnL12);
		uiBtnListL.Add (btnL13);
		uiBtnListL.Add (btnL14);
		uiBtnListL.Add (btnL15);

	
		UIEventListener.Get(homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicOnBtn).onClick = OnMusicOnBtnClick;
		UIEventListener.Get(musicOffBtn).onClick = OnMusicOffBtnClick;
	
		//界面刷新
		refreshLevelUI ();

	}



	public void PanelOff()
	{
		gameObject.SetActive (false);
	}

	void OnHomeBtnClick(GameObject btn)
	{
		//关闭当前界面，跳到主界面
		PanelOff ();
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
		
	public int GetLevel(string levelName)
	{
		int ret = 0;

		string temp =levelName.Substring (6);
		if (temp [0] == 0)
		{

			string temp2 = temp.Substring (1);
			int.TryParse (temp2, out ret);

		} 
		else
		{
			int.TryParse (temp, out ret);
		}
		//Debug.Log ("ret=="+ret);
		return ret;

	}


	//刷新UI
	public void refreshLevelUI()
	{
		

		Debug.Log (LevelManager._instance);
		for (int i = 0; i < LevelManager._instance.levelItemDataList.Count; ++i)
		{
			LevelItemData data = LevelManager._instance.levelItemDataList [i];
			Debug.Log ("refreshLevelUI==LevelId=="+data.LevelID + " " +  data.Progress);
			UIButton btnT = uiBtnListT [i];
			UIButton btnL = uiBtnListL [i];
			int levelId = GetLevel(btnT.name);
			//print ("btn.name=="+btnT.name);
			//print ("levelID==" + levelId);

			if(data.LevelID==levelId)
			{
				switch(data.Progress)
				{
					case LevelProgress.Todo:
						
						btnT.gameObject.SetActive (false);
						btnL.gameObject.SetActive (false);
						break;
					case LevelProgress.Doing:
						
						btnT.gameObject.SetActive (true);
						btnL.gameObject.SetActive (false);

						break;
					case LevelProgress.Done:
						
						btnT.gameObject.SetActive (false);
						btnL.gameObject.SetActive (true);
						break;
					default:
						break;


				}


			}
			

		


//		foreach(LevelItemData data in LevelManager._instance.levelItemDataList)
//		{
//			
//
//			switch(data.LevelID)
//			{
//			case 1:
//				
//				switch(data.Progress)
//				{
//				case LevelProgress.Todo:
//					
//				btnT01.gameObject.SetActive(false);
//				btnL01.gameObject.SetActive (false);
//					break;
//				case LevelProgress.Doing:
//					
//					btnT01.gameObject.SetActive (true);
//				btnL01.gameObject.SetActive (false);
//
//					break;
//				case LevelProgress.Done:
//					
//				
//				btnT01.gameObject.SetActive (false);
//					btnL01.gameObject.SetActive(true);
//					break;
//				default:
//					break;
//
//
//				}
//				break;
//			case 3:
//
//				switch(data.Progress)
//				{
//				case LevelProgress.Todo:
//					btnT03.gameObject.SetActive (false);
//					btnL03.gameObject.SetActive (false);
//
//					break;
//				case LevelProgress.Doing:
//					btnT03.gameObject.SetActive (true);
//					btnL03.gameObject.SetActive (false);
//					break;
//				case LevelProgress.Done:
//					btnT03.gameObject.SetActive (false);
//					btnL03.gameObject.SetActive (true);
//					break;
//				default:
//					break;
//
//
//				}
//
//				break;
//			default:
//				break;
//
//
//			}
//			
//
//		}
//
//
//	}

		}

	}
}