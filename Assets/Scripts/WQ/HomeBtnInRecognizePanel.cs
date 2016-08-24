﻿using UnityEngine;
using System.Collections;

public class HomeBtnInRecognizePanel : MonoBehaviour {

	private GameObject startPanel;




	void Start () {
		
		startPanel = transform.parent.parent.Find ("StartPanel").gameObject;


	}

	void Update () {

	}

	void OnClick()
	{
		
		//关闭父对象界面，打开主界面
		//transform.parent.gameObject.SetActive(false);
		startPanel.SetActive (true);
		transform.parent.GetComponent<PhotoRecognizingPanel> ().PanelOff();

	}
}
