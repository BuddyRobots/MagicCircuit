using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoTakingPanel : MonoBehaviour {

	public static PhotoTakingPanel _instance;


	private GameObject helpBtn;
	private GameObject confirmBtn;
	private GameObject commonPanel02;
	private GameObject demoShowPanel;
	private GameObject photoRecognizingPanel;
	private UISprite noticeImg;
	private UILabel countDown;

	private UILabel levelLabel;

	void Awake () 
	{	
		_instance = this;

		//获取需要监听的按钮对象
		helpBtn = transform.Find ("HelpBtn").gameObject;
		confirmBtn = transform.Find ("ConfirmBtn").gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		demoShowPanel = transform.parent.Find ("CommonPanel02/DemoShowPanel").gameObject;
		photoRecognizingPanel = transform.parent.Find ("PhotoRecognizingPanel").gameObject;// for test...
		noticeImg=transform.Find("Notice").GetComponent<UISprite>();
		countDown = transform.Find ("CountDown").GetComponent<UILabel> ();
		levelLabel = transform.Find ("LevelLabel").GetComponent<UILabel> ();


		//设置按钮的监听，指向本类的ButtonClick方法中。
		UIEventListener.Get(helpBtn).onClick = OnHelpBtnClick;
		UIEventListener.Get(confirmBtn).onClick = OnConfirmBtnClick;
	}

	void Start()
	{
		noticeImg.gameObject.SetActive (false);
		countDown.gameObject.SetActive (false);

	}

	void OnEnable()
	{
		levelLabel.text = LevelManager.currentLevelData.LevelName;
	}

	#region 计算按钮的点击事件


	/// <summary>
	/// 点击帮助按钮，播放操作演示
	/// </summary>
	/// <param name="btn">Button.</param>
	void OnHelpBtnClick(GameObject btn)
	{
		commonPanel02.SetActive(true);
		demoShowPanel.SetActive (true);
		PanelOff();

	}
	void OnConfirmBtnClick(GameObject btn)
	{
		//画面变暗， to do ...
	
		noticeImg.gameObject.SetActive (true);

		StartCoroutine (CountDown());//图片出来后停留几秒秒，弹出倒计时数字

	}

	IEnumerator CountDown()
	{
		yield return new WaitForSeconds (3f);

		countDown.gameObject.SetActive (true);
		//倒计时，每个数字停留一秒后变化
		yield return new WaitForSeconds(1);
		countDown.text = "2";
		yield return new WaitForSeconds (1);
		countDown.text = "1";
		yield return new WaitForSeconds (1);

		countDown.gameObject.SetActive (false);
		noticeImg.gameObject.SetActive (false);
		photoRecognizingPanel.SetActive (true);

		PanelOff ();
	}




	#endregion

	public void PanelOff()
	{
		countDown.text="3";
		gameObject.SetActive (false);

	}


}
