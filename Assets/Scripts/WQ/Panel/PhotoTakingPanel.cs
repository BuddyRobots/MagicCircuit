using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PhotoTakingPanel : MonoBehaviour 
{
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

		helpBtn = transform.Find ("HelpBtn").gameObject;
		confirmBtn = transform.Find ("ConfirmBtn").gameObject;
		commonPanel02 = transform.parent.Find ("CommonPanel02").gameObject;
		demoShowPanel = transform.parent.Find ("CommonPanel02/DemoShowPanel").gameObject;
		photoRecognizingPanel = transform.parent.Find ("PhotoRecognizingPanel").gameObject;
		noticeImg=transform.Find("Notice").GetComponent<UISprite>();
		countDown = transform.Find ("CountDown").GetComponent<UILabel> ();
		levelLabel = transform.Find ("LevelLabel").GetComponent<UILabel> ();

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
		HomeBtn.Instance.panelOff = PanelOff;
	}

	#region 计算按钮的点击事件

	void OnHelpBtnClick(GameObject btn)
	{
		commonPanel02.SetActive(true);
		demoShowPanel.SetActive (true);
		PanelOff();
	}

	void OnConfirmBtnClick(GameObject btn)
	{
		if (!noticeImg.gameObject.activeSelf) 
		{
			noticeImg.gameObject.SetActive (true);
			StartCoroutine (CountDown());//图片出来后停留几秒，弹出倒计时数字
			//CountDown();
		}
	}


	IEnumerator CountDown()
	//void CountDown()
	{
		yield return new WaitForSeconds(1f);//1f for rest, real time is 3f..

		countDown.gameObject.SetActive(true);
		//倒计时，每个数字停留一秒后变化
		yield return new WaitForSeconds(1);
		countDown.text = "2";
		yield return new WaitForSeconds(1);
		countDown.text = "1";
		yield return new WaitForSeconds(1);

		GetImage._instance.isTakingPhoto = true;
		yield return new WaitForSeconds(1);
		GetImage._instance.isTakingPhoto = false;

		GetImage._instance.Thread_Process_Start();
		//GetImage._instance.test_saveFullQuadPhotoToiPad();

		PanelOff();
		photoRecognizingPanel.SetActive (true);
	}
	#endregion

	public void PanelOff()
	{
		countDown.text = "3";
		countDown.gameObject.SetActive (false);
		noticeImg.gameObject.SetActive (false);
		gameObject.SetActive (false);
	}
}
