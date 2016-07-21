﻿using UnityEngine;
using System.Collections;

public class WelldonePanel : MonoBehaviour {


	private GameObject replayBtn;
	private GameObject nextBtn;
	private GameObject photoTakingPanel;
	//private UIButton replayBtn;
	//private UIButton nextBtn;


	// Use this for initialization
	void Start () {
	
		replayBtn = transform.Find ("ReplayBtn").GetComponent<UIButton> ().gameObject;
		nextBtn = transform.Find ("NextBtn").GetComponent<UIButton> ().gameObject;
		photoTakingPanel = transform.parent.parent.Find ("PhotoTakingPanel").gameObject;

		UIEventListener.Get (replayBtn).onClick = OnReplayBtnClick;
		UIEventListener.Get (nextBtn).onClick = OnNextBtnClick;

	}


	void OnReplayBtnClick(GameObject btn)//重玩
	{
		Debug.Log ("GameObject:" + btn.name + " is  clicked" );
		//关闭当前界面
		PanelOff();

		//重玩，返回到当前关卡的（开始拍摄界面）
		photoTakingPanel.SetActive(true);

	}
	public void PanelOff()
	{

		gameObject.SetActive(false);
		transform.parent.gameObject.SetActive (false);
	}

	void OnNextBtnClick(GameObject btn)
	{
		Debug.Log ("GameObject:" + btn.name+ " is  clicked" );
		//下一关，返回到选择关卡界面/关卡描述界面
		// to do...


		//the following is for test
		//gameObject.SetActive(false);
		//transform.parent.gameObject.SetActive(false);
		PanelOff();

		transform.parent.parent.Find ("CommonPanel01").gameObject.SetActive (true);
		transform.parent.parent.Find ("CommonPanel01/LevelSelectPanel").gameObject.SetActive (true);
		transform.parent.parent.Find ("CommonPanel01/DescriptionPanel").gameObject.SetActive (false);


	}
}
