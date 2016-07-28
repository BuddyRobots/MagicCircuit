using UnityEngine;
using System.Collections;

public class PhotoRecognizingPanel : MonoBehaviour {


	private GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject helpBtn;


	private GameObject startPanel;

	private UIAtlas atalas;

	private bool isMusicOn = true;




//	private Vector3 pos1 = new Vector3 (325f, 498f, 0);
//	private Vector3 pos2 = new Vector3 (145, 292, 0);
//	private Vector3 pos3 = new Vector3 (524, 298, 0);

	void Start () {
		homeBtn = transform.Find ("HomeBtn").GetComponent<UIButton> ().gameObject;
		musicBtn = transform.Find ("MusicBtn").GetComponent<UIButton> ().gameObject;
		helpBtn = transform.Find ("HelpBtn").GetComponent<UIButton> ().gameObject;

		startPanel = transform.parent.Find ("StartPanel").gameObject;
		atalas = Resources.Load ("Atalas/Item/Item")as UIAtlas;


		UIEventListener.Get (homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get (musicBtn).onClick = OnMusicBtnClick;
		UIEventListener.Get (helpBtn).onClick = OnHelpBtnClick;
	}
	

	void Update () {
		//Debug.Log (Input.mousePosition);

	
	}


	/// <summary>
	/// 显示图标
	/// </summary>
	void ShowItem()
	{

		//要显示图片的话需要传入坐标，先要获得图标，再根据坐标显示图标，假设坐标是下面这些
		//	private Vector3 pos1 = new Vector3 (325f, 498f, 0);
		//	private Vector3 pos2 = new Vector3 (145, 292, 0);
		//	private Vector3 pos3 = new Vector3 (524, 298, 0);
		//无背景图标电池  无背景图标闸刀
		//UISprite bulb=NGUITools.AddSprite(this,atalas,"无背景图标灯泡");


		//UISprite battery=NGUITools.AddSprite(this,atalas,"无背景图标电池");

		//UISprite OnOff=NGUITools.AddSprite(this,atalas,"无背景图标闸刀");


	}


	void OnMusicBtnClick(GameObject btn)
	{
		isMusicOn=!isMusicOn;
		musicBtn.GetComponent<UISprite> ().spriteName = (isMusicOn ? "音乐" : "静音");
		//声音开关  to do ....

	}


	void OnHomeBtnClick(GameObject btn)
	{
		Debug.Log ("OnHomeBtnClick");
		//关闭当前界面
		PanelOff ();
		//返回主界面 
		startPanel.SetActive (true);

	}

	void OnHelpBtnClick(GameObject btn)
	{
	//帮助界面
	
	}


	public void PanelOff()
	{
		gameObject.SetActive (false);

	}



}



