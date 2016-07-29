using UnityEngine;
using System.Collections;

public class PhotoTakingPanel : MonoBehaviour {

	public static PhotoTakingPanel _instance;

	private GameObject homeBtn;
	private GameObject musicBtn;
	private GameObject helpBtn;
	private GameObject confirmBtn;


	private GameObject commonPanel02;
	private GameObject demoShowPanel;
	private bool isMusicOn = true;

	void Awake () 
	{	
		_instance = this;
		//获取需要监听的按钮对象
		homeBtn = transform.Find ("HomeBtn").gameObject;
		musicBtn = transform.Find ("MusicOnBtn").gameObject;
		helpBtn = transform.Find ("HelpBtn").gameObject;
		confirmBtn = transform.Find ("ConfirmBtn").gameObject;


		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		demoShowPanel = transform.parent.Find ("CommonPanel02/DemoShowPanel").gameObject;


		//设置按钮的监听，指向本类的ButtonClick方法中。
		UIEventListener.Get(homeBtn).onClick = OnHomeBtnClick;
		UIEventListener.Get(musicBtn).onClick = OnMusicBtnClick;
		UIEventListener.Get(helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get(confirmBtn).onClick = OnConfirmBtnClick;
	}


	#region 计算按钮的点击事件

	void OnHomeBtnClick(GameObject btn)
	{
		
		//Debug.Log("GameObject " + btn.name);
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

	void OnHelpBtnClick(GameObject btn)//播放操作演示
	{
		Debug.Log("GameObject " + btn.name);
		//关闭当前界面
		PanelOff();
		//切换到帮助界面 to do...
		commonPanel02.SetActive(true);
		demoShowPanel.SetActive (true);







	}
	void OnConfirmBtnClick(GameObject btn)
	{
		//确认，画面变暗，播放倒计时动画，呈现静态照片 to do ...

		Debug.Log("GameObject " + btn.name);

		//code for test...

	}

	#endregion

	public void PanelOff()
	{
		gameObject.SetActive (false);

	}

}
